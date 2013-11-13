using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ImageByEvent.Auxiliar
{
    public static class FormaDesenhoExtensao
    {
        public static Shape NewInstance(this FormaDesenho forma)
        {
            switch (forma)
            {
                case FormaDesenho.Elipse:
                    return new Ellipse();
                case FormaDesenho.Retangulo:
                    return new Rectangle();
                default:
                    return new Ellipse();
            }
        }
    }
}
