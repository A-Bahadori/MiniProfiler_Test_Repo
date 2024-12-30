namespace MiniProfiler_Test.Models.Shared
{
    public class DeleteConfirmationViewModel
    {
        public string ModalId { get; set; } = "deleteModal";
        public string Title { get; set; }
        public string Message { get; set; }
        public string WarningMessage { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Id { get; set; }
    }
}
