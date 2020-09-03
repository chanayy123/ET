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
    unsafe class ETModel_SortingLayer_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.SortingLayer);

            field = type.GetField("VIEW", flag);
            app.RegisterCLRFieldGetter(field, get_VIEW_0);
            app.RegisterCLRFieldSetter(field, set_VIEW_0);
            field = type.GetField("WINDOW", flag);
            app.RegisterCLRFieldGetter(field, get_WINDOW_1);
            app.RegisterCLRFieldSetter(field, set_WINDOW_1);


        }



        static object get_VIEW_0(ref object o)
        {
            return ETModel.SortingLayer.VIEW;
        }
        static void set_VIEW_0(ref object o, object v)
        {
            ETModel.SortingLayer.VIEW = (System.String)v;
        }
        static object get_WINDOW_1(ref object o)
        {
            return ETModel.SortingLayer.WINDOW;
        }
        static void set_WINDOW_1(ref object o, object v)
        {
            ETModel.SortingLayer.WINDOW = (System.String)v;
        }


    }
}
