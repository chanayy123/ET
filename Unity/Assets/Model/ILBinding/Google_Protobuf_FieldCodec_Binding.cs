using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class Google_Protobuf_FieldCodec_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Google.Protobuf.FieldCodec);
            args = new Type[]{typeof(System.UInt32)};
            method = type.GetMethod("ForInt32", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ForInt32_0);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(Google.Protobuf.Adapt_IMessage.Adaptor)};
            if (genericMethods.TryGetValue("ForMessage", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(Google.Protobuf.FieldCodec<Google.Protobuf.Adapt_IMessage.Adaptor>), typeof(System.UInt32), typeof(Google.Protobuf.MessageParser<Google.Protobuf.Adapt_IMessage.Adaptor>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ForMessage_1);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ETModel.GameConfig)};
            if (genericMethods.TryGetValue("ForMessage", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(Google.Protobuf.FieldCodec<ETModel.GameConfig>), typeof(System.UInt32), typeof(Google.Protobuf.MessageParser<ETModel.GameConfig>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ForMessage_2);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ETModel.MatchRoom)};
            if (genericMethods.TryGetValue("ForMessage", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(Google.Protobuf.FieldCodec<ETModel.MatchRoom>), typeof(System.UInt32), typeof(Google.Protobuf.MessageParser<ETModel.MatchRoom>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ForMessage_3);

                        break;
                    }
                }
            }


        }


        static StackObject* ForInt32_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.UInt32 @tag = (uint)ptr_of_this_method->Value;


            var result_of_this_method = Google.Protobuf.FieldCodec.ForInt32(@tag);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ForMessage_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Google.Protobuf.MessageParser<Google.Protobuf.Adapt_IMessage.Adaptor> @parser = (Google.Protobuf.MessageParser<Google.Protobuf.Adapt_IMessage.Adaptor>)typeof(Google.Protobuf.MessageParser<Google.Protobuf.Adapt_IMessage.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.UInt32 @tag = (uint)ptr_of_this_method->Value;


            var result_of_this_method = Google.Protobuf.FieldCodec.ForMessage<Google.Protobuf.Adapt_IMessage.Adaptor>(@tag, @parser);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ForMessage_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Google.Protobuf.MessageParser<ETModel.GameConfig> @parser = (Google.Protobuf.MessageParser<ETModel.GameConfig>)typeof(Google.Protobuf.MessageParser<ETModel.GameConfig>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.UInt32 @tag = (uint)ptr_of_this_method->Value;


            var result_of_this_method = Google.Protobuf.FieldCodec.ForMessage<ETModel.GameConfig>(@tag, @parser);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ForMessage_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Google.Protobuf.MessageParser<ETModel.MatchRoom> @parser = (Google.Protobuf.MessageParser<ETModel.MatchRoom>)typeof(Google.Protobuf.MessageParser<ETModel.MatchRoom>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.UInt32 @tag = (uint)ptr_of_this_method->Value;


            var result_of_this_method = Google.Protobuf.FieldCodec.ForMessage<ETModel.MatchRoom>(@tag, @parser);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
