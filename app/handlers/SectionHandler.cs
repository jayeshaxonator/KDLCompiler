using Cottle;
using KdlDotNet;

namespace KDLCompiler
{
    internal abstract class SectionHandler : ISectionHandler
    {
        abstract protected string GetTemplateName();
            string ISectionHandler.RenderSection(KDLNode node)
        {
            string templateName = GetTemplateName();
            Dictionary<string, object> propertyCollection = new Dictionary<string, object>();
            if (node.Child == null)
            {
                Dictionary<Value, Value> valueDict0 = ConvertFrom(propertyCollection);
                return RenderTemplate(valueDict0, templateName);

            }

            
            IReadOnlyList<KDLNode> nodes = node.Child.Nodes;
            recursive(propertyCollection, nodes);
            Dictionary<Value, Value> valueDict = ConvertFrom(propertyCollection);
            return RenderTemplate(valueDict, templateName);
        }

        private Dictionary<Value, Value> ConvertFrom(Dictionary<string, object> propertyCollection)
        {
            Dictionary<Value, Value> valueDict = new Dictionary<Value, Value>();
            foreach (var item in propertyCollection)
            {
                if (item.Value is Dictionary<string, object>)
                {
                    valueDict[item.Key] = ConvertFrom((Dictionary<string, object>)item.Value);
                }
                else if (item.Value is List<object>)
                {
                    List<Value> list = new List<Value>();
                    foreach (var v in (List<object>)item.Value)
                    {
                        if (v is Dictionary<string, object>)
                        {
                            list.Add(ConvertFrom((Dictionary<string, object>)v));
                        }
                        else
                        {
                            list.Add((Value)v);
                        }
                    }
                    valueDict[item.Key] = list;
                }
                else
                {
                    KDLValue kdlValue = (KDLValue)item.Value;
                    if(kdlValue.IsNumber)
                        valueDict[item.Key] = kdlValue.AsNumber().ToString();
                    else
                    if(kdlValue.IsString)
                        valueDict[item.Key] = kdlValue.AsString().Value; 
                    else if(kdlValue.IsBoolean)
                        valueDict[item.Key] = kdlValue.AsBoolean().Value;


                }
            }
            return valueDict;
        }

        private Dictionary<string, object> recursive(Dictionary<string, object> propertyCollection, IReadOnlyList<KDLNode> nodes)
        {
            foreach (var n in nodes)
            {
                if (n.Args.Count > 0)
                {
                        propertyCollection[n.Identifier.ToString()] = n.Args[0];
                }
                else
                {
                    if (n.Child != null)
                    {
                        Dictionary<string, object> heroAttributes_child;
                        string key;
                        heroAttributes_child = new Dictionary<string, object>();
                        key = n.Identifier.ToString();
                        if (propertyCollection.ContainsKey(key))
                        {
                            object old_value = (object)propertyCollection[key];
                            string new_key = key + "s";
                            List<object> list;
                            if (propertyCollection.ContainsKey(new_key))
                            {
                                list = (List<object>)propertyCollection[new_key];
                                list.Add(old_value);
                            }
                            else
                            {
                                list = new List<object>();
                                list.Add(old_value);
                            }
                            propertyCollection[new_key] = list;
                        }
                        propertyCollection[key] = recursive(heroAttributes_child, n.Child.Nodes);
                    }
                }
            }
            return propertyCollection;
        }

       

        // string ISectionHandler.RenderSection(KDLNode node)
        // {
        //     string templateName = GetTemplateName();
        //     Dictionary<Value, Value> propertyCollection = new Dictionary<Value, Value>();
        //     if (node.Child == null)
        //         return RenderTemplate(propertyCollection, templateName);    

        //     IReadOnlyList<KDLNode> nodes = node.Child.Nodes;
        //     recursive(propertyCollection, nodes);
        //     return RenderTemplate(propertyCollection, templateName);
        // }

        // private Dictionary<Value, Value> recursive(Dictionary<Value, Value> propertyCollection, IReadOnlyList<KDLNode> nodes)
        // {
        //     foreach (var n in nodes)
        //     {
        //         if (n.Args.Count > 0)
        //         {
        //                 propertyCollection[n.Identifier.ToString()] = n.Args[0].AsString().Value;
        //         }
        //         else
        //         {
        //             if (n.Child != null)
        //             {
        //                 Dictionary<Value, Value> heroAttributes_child = new Dictionary<Value, Value>();
        //                 string key = n.Identifier.ToString();
        //                 if (propertyCollection.ContainsKey(key))
        //                 {
        //                     Value old_value = propertyCollection[key];
        //                     propertyCollection.Remove(key);
        //                     string new_key = key + "s";
        //                     Value v;
        //                     if(propertyCollection.ContainsKey(new_key))
        //                     {
        //                         v = propertyCollection[new_key];
        //                         //((Array)v).Add(old_value);
        //                     }
        //                     else 
        //                         v = new[] {old_value};
        //                     propertyCollection[new_key] = v;
        //                 }
        //                 propertyCollection[key] = recursive(heroAttributes_child, n.Child.Nodes);
        //             }
        //         }
        //     }
        //     return propertyCollection;
        // }
        private static string RenderTemplate(Dictionary<Value, Value> heroAttributes, string templateName)
        {
            string template = TemplateLoader.LoadTemplate(templateName);
            var documentResult = Document.CreateDefault(template); // Create from template string
            IDocument? document = null;
            string output;
            try {
                document = documentResult.DocumentOrThrow; // Throws ParseException on error
                IContext context = Context.CreateBuiltin(heroAttributes);
                output = document.Render(context);
            } 
            catch (Exception e) {
                output = e.Message + template;

            }
            return output;
        }
    }
}