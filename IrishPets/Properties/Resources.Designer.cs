

namespace IrishPets.Properties {
    using System;
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("IrishPets.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string DefAdmin {
            get {
                return ResourceManager.GetString("DefAdmin", resourceCulture);
            }
        }
        
        public static string DefDomain {
            get {
                return ResourceManager.GetString("DefDomain", resourceCulture);
            }
        }
        
        public static string DefEmail_Account {
            get {
                return ResourceManager.GetString("DefEmail_Account", resourceCulture);
            }
        }
        
        public static string DefEmail_Pass {
            get {
                return ResourceManager.GetString("DefEmail_Pass", resourceCulture);
            }
        }
        
        public static string DefPhone {
            get {
                return ResourceManager.GetString("DefPhone", resourceCulture);
            }
        }
        
        public static string PayPal_Email_Buyer {
            get {
                return ResourceManager.GetString("PayPal_Email_Buyer", resourceCulture);
            }
        }
        
        public static string PayPal_Email_Facilitator {
            get {
                return ResourceManager.GetString("PayPal_Email_Facilitator", resourceCulture);
            }
        }
        
        public static string PayPal_ReturnUrl {
            get {
                return ResourceManager.GetString("PayPal_ReturnUrl", resourceCulture);
            }
        }
        
        public static string PayPal_Site {
            get {
                return ResourceManager.GetString("PayPal_Site", resourceCulture);
            }
        }
    }
}
