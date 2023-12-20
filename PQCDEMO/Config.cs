using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQCDEMO
{
    public class IO_State
    {
        public byte Id { get; set; } // 添加ID属性
        public string Tag { get; set; }
        public bool State { get; set; }
    }

    public class MainConfig
    {
        public IOBoardConfig IOConfig { get; set; }
        public List<AxisConfig> AxisConfigs { get; set; }

        public MainConfig()
        {
            IOConfig = new IOBoardConfig();
            AxisConfigs = new List<AxisConfig>();
        }
    }

    public class AxisConfig
    {
        public string AxisName { get; set; }
        public UInt16 AxisStatus { get; set; }

        public AxisConfig(string axisName, UInt16 axisStatus)
        {
            AxisName = axisName;
            AxisStatus = axisStatus;
        }
    }


    public class IOBoardConfig
    {
        public List<IOItem> Inputs { get; set; }
        public List<IOItem> Outputs { get; set; }

        public IOBoardConfig()
        {
            // 初始化輸入和輸出列表
            Inputs = new List<IOItem>();
            Outputs = new List<IOItem>();
        }
    }

    public class IOItem
    {
        public byte Id { get; set; }
        public string Tag { get; set; } // 添加Tag属性
        public string Text { get; set; } // 添加Text属性
        public string Name { get; set; }
        public string Remark { get; set; }


    }



    //public class ApplicationConfig
    //{
    //    public IOBoardConfig IOConfig { get; set; }

    //    public ApplicationConfig()
    //    {
    //        // 初始化 IOBoardConfig
    //        IOConfig = new IOBoardConfig();
    //    }
    //}


    //public class ConfigurationManager
    //{
    //    private ApplicationConfig config;

    //    public ApplicationConfig GetConfiguration()
    //    {
    //        if (config == null)
    //        {
    //            // 從檔案中讀取配置或創建新的配置實例
    //            // config = DeserializeFromFile<ApplicationConfig>(...);
    //        }
    //        return config;
    //    }

    //    public void SaveConfiguration(ApplicationConfig config)
    //    {
    //        // 將配置序列化並保存到檔案
    //        // SerializeToFile(config, ...);
    //        this.config = config;
    //    }

    //    // 序列化和反序列化方法的實現依賴於您選擇的持久化策略
    //}
  
}
