using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleApi.Data.Contracts.Models.BatchProcess
{
    public class SyExecProcessInstance
    {
        public long Id { get; set; }
        public Guid SyExecProcessGId { get; set; }
        public string InstanceName { get; set; }
        public string ExecMachineHostIpAddr { get; set; }
        public string ExecMachineHostName { get; set; }
        public DateTime? ExecProcessStarted { get; set; }
        public DateTime? ExecProcessEnded { get; set; }
        public SyExecProcessStatus SyExecProcessStatusId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid SyExecProcessInstanceGId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
