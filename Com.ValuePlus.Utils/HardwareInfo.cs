using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace Com.ValuePlus.Utils
{
    public class HardwareInfo
    {

        /// <summary>
        /// 获取cpu信息
        /// </summary>
        /// <returns></returns>
      public  string GetCpuInfo()
      {
           string sCpuInfo = "";//cpu序列号
           ManagementClass cimobject = new ManagementClass("Win32_Processor");
           ManagementObjectCollection moc = cimobject.GetInstances();
           foreach(ManagementObject mo in moc)
           {
               sCpuInfo += mo.Properties["ProcessorId"].Value.ToString();
                mo.Dispose();
           }
           return sCpuInfo;
       }


        /// <summary>
        /// 获得网卡信息
        /// </summary>
        /// <returns></returns>
      public string GetNetWorkInfo()
      {
          string sNetWorkInfo = "";//cpu序列号
           ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
           ManagementObjectCollection moc2 = mc.GetInstances();
           foreach(ManagementObject mo in moc2)
           {
               if ((bool)mo["IPEnabled"] == true)
                   sNetWorkInfo += mo["MacAddress"].ToString();
                 mo.Dispose();
           }
           return sNetWorkInfo;
      }

      /// <summary> 
      /// 获取硬盘ID  
      /// </summary> 
      /// <returns>string </returns> 
      public string GetHDid()
      {
          string HDid = "";
          ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
          ManagementObjectCollection moc1 = cimobject1.GetInstances();
          foreach (ManagementObject mo in moc1)
          {
              HDid += (string)mo.Properties["Model"].Value.ToString();
              moc1.Dispose();
          }
          return HDid.ToString();
      } 

    }


}
