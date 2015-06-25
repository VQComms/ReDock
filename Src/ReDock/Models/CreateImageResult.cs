using System.Collections.Generic;

namespace ReDock
{
    public class CreateImageResult
    {
        public List<CreateImageStatusError> StatusErrors{ get; set; }

        public List<CreateImageStatusUpdate> StatusUpdates { get; set; }

        public string ImageId { get; set; }

        public CreateImageResultState State { get; set; }
    }

}
