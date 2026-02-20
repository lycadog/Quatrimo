using Quatrimo.GumRuntimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Data
{
    public interface IBlockSensable
    {


        public void GrabInfoPanel(NineSliceRuntime panel)
        {
            panel.GetChildByName("headerText");
        }


        protected SpriteRuntime SenseIcon(NineSliceRuntime panel)
        {
            return (SpriteRuntime)panel.GetChildByName("headerIcon");
        }

        protected TextRuntime SenseHeaderText(NineSliceRuntime panel)
        {
            return (TextRuntime)panel.GetChildByName("headerText");
        }


    }
}
