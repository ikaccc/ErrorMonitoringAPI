using System.Collections.Generic;

namespace ExceptionHandler.API.Models
{
    public class Request
    {
        protected readonly IDictionary<string, object> _keyedValues = (IDictionary<string, object>)new Dictionary<string, object>();
        /// <summary>Gets or sets the URL.</summary>
        /// <value>The URL.</value>
        public string Url
        {
            get => this._keyedValues["url"] as string;
            set => this._keyedValues["url"] = (object)value;
        }

        /// <summary>Gets or sets the method.</summary>
        /// <value>The method.</value>
        public string Method
        {
            get => this._keyedValues["method"] as string;
            set => this._keyedValues["method"] = (object)value;
        }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public IDictionary<string, string> Headers
        {
            get => this._keyedValues["headers"] as IDictionary<string, string>;
            set => this._keyedValues["headers"] = (object)value;
        }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public IDictionary<string, object> Params
        {
            get => this._keyedValues["params"] as IDictionary<string, object>;
            set => this._keyedValues["params"] = (object)value;
        }

        /// <summary>Gets or sets the get-parameters.</summary>
        /// <value>The get-parameters.</value>
        public IDictionary<string, object> GetParams
        {
            get => this._keyedValues["get_params"] as IDictionary<string, object>;
            set => this._keyedValues["get_params"] = (object)value;
        }

        /// <summary>Gets or sets the query string.</summary>
        /// <value>The query string.</value>
        public string QueryString
        {
            get => this._keyedValues["query_string"] as string;
            set => this._keyedValues["query_string"] = (object)value;
        }

        /// <summary>Gets or sets the post-parameters.</summary>
        /// <value>The post-parameters.</value>
        public IDictionary<string, object> PostParams
        {
            get => this._keyedValues["post_params"] as IDictionary<string, object>;
            set => this._keyedValues["post_params"] = (object)value;
        }

        /// <summary>Gets or sets the post-body.</summary>
        /// <value>The post-body.</value>
        public string PostBody
        {
            get => this._keyedValues["post_body"] as string;
            set => this._keyedValues["post_body"] = (object)value;
        }

        /// <summary>Gets or sets the user IP.</summary>
        /// <value>The user IP.</value>
        public string UserIp
        {
            get => this._keyedValues["user_ip"] as string;
            set => this._keyedValues["user_ip"] = (object)value;
        }
    }
}
