using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Ramses.Utils
{
    /// <summary>
    /// Classe contendo métodos auxiliares para facilitar rotinas básicas envolvendo enums
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Cria uma lista de EnumData com os valores e texto para exibição dos valores disponíveis em enumType
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <remarks>
        /// Para tradução do texto de exibição, é necessário que o valor do enum esteja decorado com o atributo System.ComponentModel.DataAnnotations.Display, 
        /// com o valor atribuito para propriedade Name correspondendo à uma chave no arquivo de resource
        /// </remarks>
        public static List<EnumData> TranslateValues(Type enumType)
        {
            System.Diagnostics.Debug.Assert(enumType.IsEnum);

            return (from FieldInfo e in enumType.GetFields(BindingFlags.Static | BindingFlags.Public)
                    select new EnumData() 
                                { 
                                    Value = e.GetRawConstantValue(),
                                    Name = e.Name,
                                    Display = e.Name /*Attribute.GetCustomAttribute(e, typeof(DisplayAttribute)) == null
                                                    ? e.Name 
                                                    : (Resources.Literals.ResourceManager.GetString(((DisplayAttribute)Attribute.GetCustomAttribute(e, typeof(DisplayAttribute), false)).Name) 
                                                        ?? ((DisplayAttribute)Attribute.GetCustomAttribute(e, typeof(DisplayAttribute), false)).Name)*/
                                }).ToList();
        }

    }
}