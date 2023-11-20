using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPM;

namespace PQCDEMO
{
    public class MotionController
    {
        private ushort switchNo;
        private Master.PCI_M114.ErrCode ret;
        private bool _demo = false;

        public MotionController(bool demo)
        {
            _demo = demo;
            if (!demo) { InitializeHardware(); }
      
        }

        // 初始化硬件的方法
        private void InitializeHardware()
        {
            UInt16 existCards = 0;
            ret = Master.PCI_M114._m114_open(ref existCards);
            if (existCards == 0 || ret != Master.PCI_M114.ErrCode.ERR_NoError)
            {
                MessageBox.Show("未找到 PCI_M114 卡或打開卡時出錯。", "錯誤");
                //throw new Exception("初始化失敗。");
            }

            ret = Master.PCI_M114._m114_get_switch_card_num(0, ref switchNo);
            if (ret != Master.PCI_M114.ErrCode.ERR_NoError)
            {
                MessageBox.Show("獲取切換卡號時出錯。", "錯誤");
                //throw new Exception("初始化失敗。");
            }

            ret = Master.PCI_M114._m114_initial(switchNo);
            if (ret != Master.PCI_M114.ErrCode.ERR_NoError)
            {
                MessageBox.Show("初始化卡時出錯。", "錯誤");
               // throw new Exception("初始化失敗。");
            }
        }

        // 獲取特定軸狀態的方法
        public bool GetAxisStatus(ushort axis, out UInt16 axisStatus)
        {
            axisStatus = 0;
            if (_demo) {
                axisStatus = 340; // 如果 demo 为 true，则设置 axisStatus 为 65536
                return true;
            }
            ret  = Master.PCI_M114._m114_get_io_status(switchNo, axis, ref axisStatus);
            if (ret != Master.PCI_M114.ErrCode.ERR_NoError)
            {
                MessageBox.Show("GetAxisStatus失敗", "錯誤");

                return false;
            }
            return true;
        }

        // 根據需要添加更多與硬件交互的方法
    }

}
