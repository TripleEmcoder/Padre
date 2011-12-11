using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNet
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            A(1, "test");
        }

        private static void A(int i, string t)
        {
            B(i + 2, t + " 2");
        }

        private static void B(int i, string t)
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception exception)
            {
            }
        }
    }
}
