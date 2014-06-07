using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ramses.Utils
{
    /// <summary>
    /// Associação entre valores de enum e um texto para exibição desse valor na tela
    /// </summary>
    public class EnumData
    {
        /// <summary>
        /// Valor original definido no enum
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Valor original definido no enum
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Texto de exibição do valor do enum
        /// </summary>
        public string Display { get; set; }
    }
}