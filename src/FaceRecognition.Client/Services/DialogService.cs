using System;
using System.Collections.Generic;

namespace FaceRecognition.Client.Services
{
    class DialogService : IOService
    {
        public string Open()
        {
            return "test";
        }

        public List<string> OpenMultiple()
        {
            List<string> paths = new();
            return paths;
        }
    }
}