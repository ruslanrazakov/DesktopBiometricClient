
using System;
using System.Collections.Generic;

namespace FaceRecognition.Client.Services
{
    interface IOService
    {
        string Open();
        List<string> OpenMultiple();
    }
}