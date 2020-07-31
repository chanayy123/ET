using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace ETModel
{

    public interface ISession
    {
        object this[string name] { get; set; }
        void Remove(string name);
    }

    public interface IPrincipal
    {
        bool IsInRole(string role);
        IIdentity Identity { get; }
    }

    public class HttpSession : ISession
    {
        private readonly Dictionary<string, object> _dic = new Dictionary<string, object>();
        public object this[string name]
        {
            get
            {
                _dic.TryGetValue(name, out object value);
                return value;
            }
            set
            {
                _dic[name] = value;
            }
        }

        public void Remove(string name)
        {
            _dic.Remove(name);
        }
    }

    public class AnonymousIdentity : IIdentity
    {
        public string AuthenticationType => "";

        public bool IsAuthenticated => false;

        public string Name => "Anonymous";
    }

    public class AnonymousUser : IPrincipal
    {
        public IIdentity Identity => new AnonymousIdentity();

        public bool IsInRole(string role)
        {
            return false;
        }
    }

    public class GenericUser : IPrincipal
    {
        private readonly string _name;
        private int _level;
        public GenericUser(string name,int level)
        {
            _name = name;
            _level = level;
        }
        public IIdentity Identity => new GenericIdentity(_name);

        public bool IsInRole(string role)
        {
            return false;
        }
        public string Name => _name;

        public int Level => _level;

    }

    public class HttpServerContext
    {
        private readonly HttpListenerContext _context;
        public HttpListenerRequest Request => _context.Request;
        public HttpListenerResponse Response => _context.Response;

        public IPrincipal User { get; internal set; }
        public ISession Session { get; internal set; }

        public HttpServerContext(HttpListenerContext context)
        {
            _context = context;
        }
    }
}
