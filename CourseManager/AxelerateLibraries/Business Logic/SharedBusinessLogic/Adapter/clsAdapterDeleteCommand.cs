using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Axelerate.BusinessLayerFrameWork.BLCore;
using Axelerate.BusinessLayerFrameWork.DLCore;
using Axelerate.BusinessLayerFrameWork.BLCore.DataTypes;
using System.Reflection;

namespace Axelerate.BusinessLogic.SharedBusinessLogic.Adapters
{
    class clsAdapterDeleteCommand : clsAdapterCommandBase
    {
        #region "Constructors"
        public clsAdapterDeleteCommand(clsAdapterDataLayer pDataLayer, BLBusinessBase NewBusinessObject)
            : base(pDataLayer, NewBusinessObject)
        {
        }
        #endregion

        #region "DataLayerCommandBase Overrides"
        protected override void OnExecute()
        {
            clsAdapterBase AdapterInstance = DataLayer.AdapterInstance;
            string InserMethodName = AdapterInstance.GetMethodName(clsAdapterBase.AdapterMethodType.AdapaterDelete);

            object[] ParamsValues = new object[DataLayer.FieldMapList.KeyFields.Count];
            int i = 0;
            foreach (BLFieldMap param in DataLayer.FieldMapList.KeyFields)
            {
                ParamsValues[i] = BusinessObject.get_FieldValue(param.Field);
                i++;
            }

            DataLayer.AdapterType.InvokeMember(InserMethodName,
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy | BindingFlags.Public,
                null, AdapterInstance, ParamsValues); 
        }

        public override void Finish()
        {
            m_CommandState = CommandStateType.Finished;
        }
        #endregion
    }
}