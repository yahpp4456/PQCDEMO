using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TPM;


namespace TPM
{
    class PCE_D122
    {
        private const string DLLNAME = "D122_x64.dll";

        // Error Table
        public enum ErrCode : short
        {
            D122ERR_NoError = 0,
            D122ERR_InvalidSwitchCardNumber = -1,
            D122ERR_SwitchCardNumberRepeated = -2,
            D122ERR_MapMemoryFailed = -3,
            D122ERR_CardNotExist = -4,
            D122ERR_InvalidBoardID = -5,
            D122ERR_InvalidParameter1 = -6,
            D122ERR_InvalidParameter2 = -7,
            D122ERR_InvalidParameter3 = -8,
            D122ERR_InvalidParameter4 = -9,
            D122ERR_InvalidParameter5 = -10,
            D122ERR_InvalidParameter6 = -11,
            D122ERR_InvalidParameter7 = -12,
            D122ERR_InvalidParameter8 = -13,
            D122ERR_InvalidParameter9 = -14,
        }

        public enum CardType : short
        {
            CARD_UNKNOWN = 0,
            CARD_PCE_D122 = 1,
            CARD_PCE_D132 = 2,
            CARD_PCE_D150 = 3,
            CARD_PCE_D105 = 4,
        }

        [DllImport(DLLNAME)]
        public static extern short _d122_open(ref ushort ExistCards);
        [DllImport(DLLNAME)]
        public static extern short _d122_close();
        [DllImport(DLLNAME)]
        public static extern short _d122_check_switch_card_num(ushort SwitchCardNo, ref byte IsExist);
        [DllImport(DLLNAME)]
        public static extern short _d122_get_switch_card_num(ushort CardIndex, ref ushort SwitchCardNo);
        [DllImport(DLLNAME)]
        public static extern short _d122_get_card_type(ushort SwitchCardNo, ref byte CardType);
        [DllImport(DLLNAME)]
        public static extern short _d122_get_cpld_version(ushort SwitchCardNo, ref ushort CpldVer);

        // read / write all

        // U8 InData[]
        //   PCE-D122: InData[4]
        //   PCE-D132: InData[6]
        //   PCE-D150: InData[10]
        //   PCE-D105: NotSupported

        [DllImport(DLLNAME)]
        public static extern short _d122_read_input(ushort SwitchCardNo, byte[] InData);

        // U8 OutData[]
        //   PCE-D122: OutData[4]
        //   PCE-D132: OutData[4]
        //   PCE-D150: NotSupported
        //   PCE-D105: OutData[10]
        [DllImport(DLLNAME)]
        public static extern short _d122_read_output(ushort SwitchCardNo, byte[] InData);
        [DllImport(DLLNAME)]
        public static extern short _d122_write_output(ushort SwitchCardNo, byte[] InData);

        // read / write a bit
        // U8 InBitNo
        //   PCE-D122: 0 ~ 31
        //   PCE-D132: 0 ~ 47
        //   PCE-D150: 0 ~ 79
        //   PCE-D105: NotSupported
        [DllImport(DLLNAME)]
        public static extern short _d122_read_input_bit(ushort SwitchCardNo, byte BitNo, ref byte OnOff);
        // U8 OutBitNo
        //   PCE-D122: 0 ~ 31
        //   PCE-D132: 0 ~ 31
        //   PCE-D150: NotSupported
        //   PCE-D105: 0 ~ 79
        [DllImport(DLLNAME)]
        public static extern short _d122_read_output_bit(ushort SwitchCardNo, byte BitNo, ref byte OnOff);
        [DllImport(DLLNAME)]
        public static extern short _d122_write_output_bit(ushort SwitchCardNo, byte BitNo, byte OnOff);
        [DllImport(DLLNAME)]
        public static extern short _d122_toggle_output_bit(ushort SwitchCardNo, byte BitNo);

        // read / write a byte
        // U8 PortNo
        //   PCE-D122: 0 ~ 7
        //   PCE-D132: 0 ~ 9
        //   PCE-D150: 0 ~ 9
        //   PCE-D105: 0 ~ 9
        //
        //        D122        D132        D150        D105
        // Port0: DI[ 0.. 7]  DI[ 0.. 7]  DI[ 0.. 7]  DO[ 0.. 7]
        // Port1: DI[ 8..15]  DI[ 8..15]  DI[ 8..15]  DO[ 8..15]
        // Port2: DI[16..23]  DI[16..23]  DI[16..23]  DO[16..23]
        // Port3: DI[24..31]  DI[24..31]  DI[24..31]  DO[24..31]
        // Port4: DO[ 0.. 7]  DI[32..39]  DI[32..39]  DO[32..39]
        // Port5: DO[ 8..15]  DI[40..47]  DI[40..47]  DO[40..47]
        // Port6: DO[16..23]  DO[ 0.. 7]  DI[48..55]  DO[48..55]
        // Port7: DO[24..31]  DO[ 8..15]  DI[56..63]  DO[56..63]
        // Port8:             DO[16..23]  DI[64..71]  DO[64..71]
        // Port9:             DO[24..31]  DI[72..79]  DO[72..79]
        [DllImport(DLLNAME)]
        public static extern short _d122_read_port(ushort SwitchCardNo, byte PortNo, ref byte Value);
        [DllImport(DLLNAME)]
        public static extern short _d122_write_port(ushort SwitchCardNo, byte PortNo, byte Value);
    }
}

namespace PQCDEMO
{
    public class IOCardWrapper
    {
        private const string DLLNAME = "D122_x64.dll";
        private ushort m_CardNo = 0;
        private bool isDemoMode = false;
        private Dictionary<byte, bool> demoInputValues = new Dictionary<byte, bool>();

        public enum ErrCode : short
        {
            D122ERR_NoError = 0,
            // 定義其他錯誤代碼...
        }

        public IOCardWrapper(bool demoMode)
        {
            isDemoMode = demoMode;
            Initialize();
        }

        private void Initialize()
        {
            if (!isDemoMode)
            {
                ushort existCards = 0;
                short result = PCE_D122._d122_open(ref existCards);

                if (result == (short)ErrCode.D122ERR_NoError && existCards > 0)
                {
                    result = PCE_D122._d122_get_switch_card_num(0, ref m_CardNo);
                }
                else
                {
                    throw new Exception("IO卡初始化失敗");
                }
            }
        }

        public void Close()
        {
            if (!isDemoMode)
            {
                PCE_D122._d122_close();
            }
        }

        public bool ReadInputBit(byte bitNo)
        {
            if (isDemoMode)
            {
                // 在DEMO模式下檢查是否有設定預設值
                if (demoInputValues.ContainsKey(bitNo))
                {
                    return demoInputValues[bitNo];
                }
                else
                {
                    // 如果沒有設定預設值，則返回虛擬數據
                    return SimulateInputBit(bitNo);
                }
            }
            else
            {
                byte value = 0;
                short result = PCE_D122._d122_read_input_bit(m_CardNo, bitNo, ref value);

                if (result == (short)ErrCode.D122ERR_NoError)
                {
                    return value == 1;
                }
                else
                {
                    throw new Exception("讀取輸入位元失敗");
                }
            }
        }

        public void SetOutputBit(byte bitNo, bool state)
        {
            if (isDemoMode)
            {
                SimulateSetOutputBit(bitNo, state);
            }
            else
            {
                byte onOff = (byte)(state ? 1 : 0);
                short result = PCE_D122._d122_write_output_bit(m_CardNo, bitNo, onOff);

                if (result != (short)ErrCode.D122ERR_NoError)
                {
                    throw new Exception("設置輸出位元狀態失敗");
                }
            }
        }

        public void ToggleOutputBit(byte bitNo)
        {
            if (isDemoMode)
            {
                SimulateToggleOutputBit(bitNo);
            }
            else
            {
                short result = PCE_D122._d122_toggle_output_bit(m_CardNo, bitNo);

                if (result != (short)ErrCode.D122ERR_NoError)
                {
                    throw new Exception("切換輸出位元狀態失敗");
                }
            }
        }

        // 新增方法，用於設定 DEMO 模式下的預設輸入值
        public void SetDemoInputValue(byte bitNo, bool value)
        {
            if (isDemoMode)
            {
                if (demoInputValues.ContainsKey(bitNo))
                {
                    demoInputValues[bitNo] = value;
                }
                else
                {
                    demoInputValues.Add(bitNo, value);
                }
            }
        }

        private bool SimulateInputBit(byte bitNo)
        {
            // 在DEMO模式下模擬讀取輸入位元的邏輯，例如返回虛擬數據
            return false;
        }

        private void SimulateSetOutputBit(byte bitNo, bool state)
        {
            // 在DEMO模式下模擬設置輸出位元的邏輯，例如記錄設置操作
        }

        private void SimulateToggleOutputBit(byte bitNo)
        {
            // 在DEMO模式下模擬切換輸出位元的邏輯，例如切換虛擬位元的狀態
        }
    }

}
