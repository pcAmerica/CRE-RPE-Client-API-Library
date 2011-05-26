﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pcAmerica.DesktopPOS.API.Client.EmployeeService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Employee", Namespace="http://pcAmerica.com/DesktopPOS/EmployeeService/DataContracts/2010/07/27")]
    [System.SerializableAttribute()]
    public partial class Employee : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AccessLevelField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CashierIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreateDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DisplayNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AccessLevel {
            get {
                return this.AccessLevelField;
            }
            set {
                if ((this.AccessLevelField.Equals(value) != true)) {
                    this.AccessLevelField = value;
                    this.RaisePropertyChanged("AccessLevel");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CashierID {
            get {
                return this.CashierIDField;
            }
            set {
                if ((object.ReferenceEquals(this.CashierIDField, value) != true)) {
                    this.CashierIDField = value;
                    this.RaisePropertyChanged("CashierID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime CreateDate {
            get {
                return this.CreateDateField;
            }
            set {
                if ((this.CreateDateField.Equals(value) != true)) {
                    this.CreateDateField = value;
                    this.RaisePropertyChanged("CreateDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DisplayName {
            get {
                return this.DisplayNameField;
            }
            set {
                if ((object.ReferenceEquals(this.DisplayNameField, value) != true)) {
                    this.DisplayNameField = value;
                    this.RaisePropertyChanged("DisplayName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName {
            get {
                return this.FirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true)) {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName {
            get {
                return this.LastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LastNameField, value) != true)) {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://pcAmerica.com/DesktopPOS/EmployeeService/ServiceContracts/2010/07/27", ConfigurationName="EmployeeService.EmployeeService")]
    public interface EmployeeService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://pcAmerica.com/DesktopPOS/EmployeeService/ServiceContracts/2010/07/27/Emplo" +
            "yeeService/GetCurrentUser", ReplyAction="http://pcAmerica.com/DesktopPOS/EmployeeService/ServiceContracts/2010/07/27/Emplo" +
            "yeeService/GetCurrentUserResponse")]
        pcAmerica.DesktopPOS.API.Client.EmployeeService.Employee GetCurrentUser();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://pcAmerica.com/DesktopPOS/EmployeeService/ServiceContracts/2010/07/27/Emplo" +
            "yeeService/AuthenticateEmployee", ReplyAction="http://pcAmerica.com/DesktopPOS/EmployeeService/ServiceContracts/2010/07/27/Emplo" +
            "yeeService/AuthenticateEmployeeResponse")]
        pcAmerica.DesktopPOS.API.Client.EmployeeService.Employee AuthenticateEmployee(string userName, string password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface EmployeeServiceChannel : pcAmerica.DesktopPOS.API.Client.EmployeeService.EmployeeService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EmployeeServiceClient : System.ServiceModel.ClientBase<pcAmerica.DesktopPOS.API.Client.EmployeeService.EmployeeService>, pcAmerica.DesktopPOS.API.Client.EmployeeService.EmployeeService {
        
        public EmployeeServiceClient() {
        }
        
        public EmployeeServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EmployeeServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmployeeServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmployeeServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public pcAmerica.DesktopPOS.API.Client.EmployeeService.Employee GetCurrentUser() {
            return base.Channel.GetCurrentUser();
        }
        
        public pcAmerica.DesktopPOS.API.Client.EmployeeService.Employee AuthenticateEmployee(string userName, string password) {
            return base.Channel.AuthenticateEmployee(userName, password);
        }
    }
}
