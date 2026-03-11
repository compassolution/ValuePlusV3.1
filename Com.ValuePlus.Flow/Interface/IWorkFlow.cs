using System;
using System.Text;
using Com.ValuePlus.Flow.Entity;
using System.Collections;

namespace Com.ValuePlus.Flow.Interface
{
    public interface IWorkFlow
    {

        ///// <summary>
        ///// 创建流程实例
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public static String doCreateWorkFlow(Entity_TB_FLOW_WORK_INSTANCE entity)
        //{

        //}

        ///// <summary>
        ///// 更新流程实例
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public static String doUpdateWorkFlow(Entity_TB_FLOW_WORK_INSTANCE entity)
        //{
        //}


        ///// <summary>
        ///// 获取某一流程实例的流程状态
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        ///// <returns></returns>
        //public static void doGetWorkFlowState(String strWorkFlowCode)
        //{
        //}

        ///// <summary>
        ///// 设置某一流程实例的流程状态
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        ///// <returns></returns>
        //public static void doSetWorkFlowState(String strWorkFlowCode)
        //{
        //}

        ///// <summary>
        ///// 设置某一流程实例的流程业务状态
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        ///// <param name="strBizState"></param>
        //public static void doSetBizState(String strWorkFlowCode, String strBizState)
        //{
        //}

        ///// <summary>
        ///// 获取某一流程实例的流程业务状态
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        ///// <param name="strBizState"></param>
        //public static void doSetBizState(String strWorkFlowCode)
        //{

        //}

        ///// <summary>
        ///// 删除某一流程实例
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        //public static void doDeleteWorkFlow(String strWorkFlowCode)
        //{

        //}

        ///// <summary>
        ///// 判断某一流程实例是否完成
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        ///// <returns></returns>
        //public static bool isWorkflowFinished(String strWorkFlowCode)
        //{

        //}
        
        ///// <summary>
        ///// 根据某一流程实例获取其对应实体ID
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        ///// <returns></returns>
        //public static String doGetEntityIdByFlowId(String strWorkFlowCode)
        //{

        //}

        ///// <summary>
        ///// 根据某一流程实例返回该流程实例的当前所处岗位ID
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        ///// <returns></returns>
        //public static int doGetCurStepIdByFlowId(String strWorkFlowCode)
        //{
        //}

        ///// <summary>
        ///// 根据流程实例返回该流程实例的对应流程定义的岗位列表
        ///// </summary>
        ///// <param name="strWorkFlowCode"></param>
        ///// <returns></returns>
        //public static ArrayList doGetFlowStepByFlowId(String strWorkFlowCode)
        //{
        //}

    }
}
