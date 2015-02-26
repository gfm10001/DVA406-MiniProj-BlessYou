using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Detector
{
    class CBRSystem
    {
        



        public CBRSystem()
        { }




        public bool Evaluate(string filepath)
        {
            WavFile wr = new WavFile(filepath);
            //wr.ReadFile(filepath);
            throw new NotImplementedException();
        }



        public void ResetLib()
        {
            throw new NotImplementedException();
        }
    }
}
