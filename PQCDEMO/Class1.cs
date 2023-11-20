using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQCDEMO
{

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
        public string Name { get; set; }

    }

 
    public class ApplicationConfig
    {
        public IOBoardConfig IOB { get; set; }

        public ApplicationConfig()
        {
            // 初始化 IOBoardConfig
            IOB = new IOBoardConfig();
        }
    }


    public class ConfigurationManager
    {
        private ApplicationConfig config;

        public ApplicationConfig GetConfiguration()
        {
            if (config == null)
            {
                // 從檔案中讀取配置或創建新的配置實例
                // config = DeserializeFromFile<ApplicationConfig>(...);
            }
            return config;
        }

        public void SaveConfiguration(ApplicationConfig config)
        {
            // 將配置序列化並保存到檔案
            // SerializeToFile(config, ...);
            this.config = config;
        }

        // 序列化和反序列化方法的實現依賴於您選擇的持久化策略
    }





}
