using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTranslator
{
    static class CommonHelpers
    {
        public static bool IsFormDisposing(Form form)
        {
            return form.IsDisposed || form.Disposing;
        }
    }
}
