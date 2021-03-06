﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pcAmerica.DesktopPOS.API.Client.PaymentService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CreditCardRequest", Namespace="http://pcAmerica.com/DesktopPOS/PaymentService/DataContracts/2009/10/26")]
    [System.SerializableAttribute()]
    public partial class CreditCardRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool BarTabField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CardNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CardSwipeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ExpirationMonthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ExpirationYearField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PaymentIndexField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostAuthReferenceNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private pcAmerica.DesktopPOS.API.Client.PaymentService.ProcessingType ProcessingTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReferenceNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal TipAmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TransactionNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool VoidField;
        
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
        public decimal Amount {
            get {
                return this.AmountField;
            }
            set {
                if ((this.AmountField.Equals(value) != true)) {
                    this.AmountField = value;
                    this.RaisePropertyChanged("Amount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool BarTab {
            get {
                return this.BarTabField;
            }
            set {
                if ((this.BarTabField.Equals(value) != true)) {
                    this.BarTabField = value;
                    this.RaisePropertyChanged("BarTab");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CardNumber {
            get {
                return this.CardNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.CardNumberField, value) != true)) {
                    this.CardNumberField = value;
                    this.RaisePropertyChanged("CardNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CardSwipe {
            get {
                return this.CardSwipeField;
            }
            set {
                if ((object.ReferenceEquals(this.CardSwipeField, value) != true)) {
                    this.CardSwipeField = value;
                    this.RaisePropertyChanged("CardSwipe");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ExpirationMonth {
            get {
                return this.ExpirationMonthField;
            }
            set {
                if ((this.ExpirationMonthField.Equals(value) != true)) {
                    this.ExpirationMonthField = value;
                    this.RaisePropertyChanged("ExpirationMonth");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ExpirationYear {
            get {
                return this.ExpirationYearField;
            }
            set {
                if ((this.ExpirationYearField.Equals(value) != true)) {
                    this.ExpirationYearField = value;
                    this.RaisePropertyChanged("ExpirationYear");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PaymentIndex {
            get {
                return this.PaymentIndexField;
            }
            set {
                if ((this.PaymentIndexField.Equals(value) != true)) {
                    this.PaymentIndexField = value;
                    this.RaisePropertyChanged("PaymentIndex");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PostAuthReferenceNumber {
            get {
                return this.PostAuthReferenceNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.PostAuthReferenceNumberField, value) != true)) {
                    this.PostAuthReferenceNumberField = value;
                    this.RaisePropertyChanged("PostAuthReferenceNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public pcAmerica.DesktopPOS.API.Client.PaymentService.ProcessingType ProcessingType {
            get {
                return this.ProcessingTypeField;
            }
            set {
                if ((this.ProcessingTypeField.Equals(value) != true)) {
                    this.ProcessingTypeField = value;
                    this.RaisePropertyChanged("ProcessingType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReferenceNumber {
            get {
                return this.ReferenceNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.ReferenceNumberField, value) != true)) {
                    this.ReferenceNumberField = value;
                    this.RaisePropertyChanged("ReferenceNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal TipAmount {
            get {
                return this.TipAmountField;
            }
            set {
                if ((this.TipAmountField.Equals(value) != true)) {
                    this.TipAmountField = value;
                    this.RaisePropertyChanged("TipAmount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TransactionNumber {
            get {
                return this.TransactionNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.TransactionNumberField, value) != true)) {
                    this.TransactionNumberField = value;
                    this.RaisePropertyChanged("TransactionNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Void {
            get {
                return this.VoidField;
            }
            set {
                if ((this.VoidField.Equals(value) != true)) {
                    this.VoidField = value;
                    this.RaisePropertyChanged("Void");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProcessingType", Namespace="http://pcAmerica.com/DesktopPOS/General/DataContracts/2009/10/26")]
    public enum ProcessingType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Sale = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PreAuth = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PostAuth = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Force = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Credit = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        VoidSale = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        VoidCredit = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        VoidPreAuth = 7,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CreditCardPaymentProcessingResponse", Namespace="http://pcAmerica.com/DesktopPOS/PaymentService/DataContracts/2009/10/26")]
    [System.SerializableAttribute()]
    public partial class CreditCardPaymentProcessingResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ApprovalCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CardNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ExpirationMonthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ExpirationYearField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsPrePaidCardField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostAuthReferenceNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private pcAmerica.DesktopPOS.API.Client.PaymentService.ProcessingType ProcessTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReferenceNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ResultField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal TipAmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long TransactionNumberField;
        
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
        public decimal Amount {
            get {
                return this.AmountField;
            }
            set {
                if ((this.AmountField.Equals(value) != true)) {
                    this.AmountField = value;
                    this.RaisePropertyChanged("Amount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApprovalCode {
            get {
                return this.ApprovalCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.ApprovalCodeField, value) != true)) {
                    this.ApprovalCodeField = value;
                    this.RaisePropertyChanged("ApprovalCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CardNumber {
            get {
                return this.CardNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.CardNumberField, value) != true)) {
                    this.CardNumberField = value;
                    this.RaisePropertyChanged("CardNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ExpirationMonth {
            get {
                return this.ExpirationMonthField;
            }
            set {
                if ((this.ExpirationMonthField.Equals(value) != true)) {
                    this.ExpirationMonthField = value;
                    this.RaisePropertyChanged("ExpirationMonth");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ExpirationYear {
            get {
                return this.ExpirationYearField;
            }
            set {
                if ((this.ExpirationYearField.Equals(value) != true)) {
                    this.ExpirationYearField = value;
                    this.RaisePropertyChanged("ExpirationYear");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsPrePaidCard {
            get {
                return this.IsPrePaidCardField;
            }
            set {
                if ((this.IsPrePaidCardField.Equals(value) != true)) {
                    this.IsPrePaidCardField = value;
                    this.RaisePropertyChanged("IsPrePaidCard");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PostAuthReferenceNumber {
            get {
                return this.PostAuthReferenceNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.PostAuthReferenceNumberField, value) != true)) {
                    this.PostAuthReferenceNumberField = value;
                    this.RaisePropertyChanged("PostAuthReferenceNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public pcAmerica.DesktopPOS.API.Client.PaymentService.ProcessingType ProcessType {
            get {
                return this.ProcessTypeField;
            }
            set {
                if ((this.ProcessTypeField.Equals(value) != true)) {
                    this.ProcessTypeField = value;
                    this.RaisePropertyChanged("ProcessType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReferenceNumber {
            get {
                return this.ReferenceNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.ReferenceNumberField, value) != true)) {
                    this.ReferenceNumberField = value;
                    this.RaisePropertyChanged("ReferenceNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Result {
            get {
                return this.ResultField;
            }
            set {
                if ((this.ResultField.Equals(value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal TipAmount {
            get {
                return this.TipAmountField;
            }
            set {
                if ((this.TipAmountField.Equals(value) != true)) {
                    this.TipAmountField = value;
                    this.RaisePropertyChanged("TipAmount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long TransactionNumber {
            get {
                return this.TransactionNumberField;
            }
            set {
                if ((this.TransactionNumberField.Equals(value) != true)) {
                    this.TransactionNumberField = value;
                    this.RaisePropertyChanged("TransactionNumber");
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://pcAmerica.com/DesktopPOS/PaymentService/ServiceContracts/2009/10/26", ConfigurationName="PaymentService.PaymentService")]
    public interface PaymentService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://pcAmerica.com/DesktopPOS/PaymentService/ServiceContracts/2009/10/26/Paymen" +
            "tService/ProcessCreditCard", ReplyAction="http://pcAmerica.com/DesktopPOS/PaymentService/ServiceContracts/2009/10/26/Paymen" +
            "tService/ProcessCreditCardResponse")]
        pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardPaymentProcessingResponse ProcessCreditCard(pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://pcAmerica.com/DesktopPOS/PaymentService/ServiceContracts/2009/10/26/Paymen" +
            "tService/CompletePreAuth", ReplyAction="http://pcAmerica.com/DesktopPOS/PaymentService/ServiceContracts/2009/10/26/Paymen" +
            "tService/CompletePreAuthResponse")]
        pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardPaymentProcessingResponse CompletePreAuth(pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest request, long invoiceNumber);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface PaymentServiceChannel : pcAmerica.DesktopPOS.API.Client.PaymentService.PaymentService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PaymentServiceClient : System.ServiceModel.ClientBase<pcAmerica.DesktopPOS.API.Client.PaymentService.PaymentService>, pcAmerica.DesktopPOS.API.Client.PaymentService.PaymentService {
        
        public PaymentServiceClient() {
        }
        
        public PaymentServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PaymentServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PaymentServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PaymentServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardPaymentProcessingResponse ProcessCreditCard(pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest request) {
            return base.Channel.ProcessCreditCard(request);
        }
        
        public pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardPaymentProcessingResponse CompletePreAuth(pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest request, long invoiceNumber) {
            return base.Channel.CompletePreAuth(request, invoiceNumber);
        }
    }
}
