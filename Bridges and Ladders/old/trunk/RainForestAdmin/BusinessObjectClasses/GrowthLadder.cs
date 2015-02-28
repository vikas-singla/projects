using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RainForestAdmin.BusinessObjectClasses
{
    public class GrowthLadder
    {
       
            private string _position;

            public string Position
            {
                get { return _position; }
                set { _position = value; }
            }
            private string _designation;

            public string Designation
            {
                get { return _designation; }
                set { _designation = value; }
            }
            private string _compensation;

            public string CompensationRange
            {
                get { return _compensation; }
                set { _compensation = value; }
            }
       
    }
}