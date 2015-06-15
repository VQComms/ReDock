namespace ReDock
{
    public abstract class ContainerActionResult
    {
        public string ContainerId { get; set; }

        public ContainerResultState Result { get; set; }

        protected ContainerActionResult(string ContainerId)
        {
            this.ContainerId = ContainerId;    
        }
    }
}
