// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: proto/services/request/topicSubscription.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ubii.Services.Request {

  /// <summary>Holder for reflection information generated from proto/services/request/topicSubscription.proto</summary>
  public static partial class TopicSubscriptionReflection {

    #region Descriptor
    /// <summary>File descriptor for proto/services/request/topicSubscription.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static TopicSubscriptionReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ci5wcm90by9zZXJ2aWNlcy9yZXF1ZXN0L3RvcGljU3Vic2NyaXB0aW9uLnBy",
            "b3RvEhV1YmlpLnNlcnZpY2VzLnJlcXVlc3QiXAoRVG9waWNTdWJzY3JpcHRp",
            "b24SEQoJY2xpZW50X2lkGAEgASgJEhgKEHN1YnNjcmliZV90b3BpY3MYAiAD",
            "KAkSGgoSdW5zdWJzY3JpYmVfdG9waWNzGAMgAygJYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ubii.Services.Request.TopicSubscription), global::Ubii.Services.Request.TopicSubscription.Parser, new[]{ "ClientId", "SubscribeTopics", "UnsubscribeTopics" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class TopicSubscription : pb::IMessage<TopicSubscription> {
    private static readonly pb::MessageParser<TopicSubscription> _parser = new pb::MessageParser<TopicSubscription>(() => new TopicSubscription());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<TopicSubscription> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ubii.Services.Request.TopicSubscriptionReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TopicSubscription() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TopicSubscription(TopicSubscription other) : this() {
      clientId_ = other.clientId_;
      subscribeTopics_ = other.subscribeTopics_.Clone();
      unsubscribeTopics_ = other.unsubscribeTopics_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TopicSubscription Clone() {
      return new TopicSubscription(this);
    }

    /// <summary>Field number for the "client_id" field.</summary>
    public const int ClientIdFieldNumber = 1;
    private string clientId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ClientId {
      get { return clientId_; }
      set {
        clientId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "subscribe_topics" field.</summary>
    public const int SubscribeTopicsFieldNumber = 2;
    private static readonly pb::FieldCodec<string> _repeated_subscribeTopics_codec
        = pb::FieldCodec.ForString(18);
    private readonly pbc::RepeatedField<string> subscribeTopics_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> SubscribeTopics {
      get { return subscribeTopics_; }
    }

    /// <summary>Field number for the "unsubscribe_topics" field.</summary>
    public const int UnsubscribeTopicsFieldNumber = 3;
    private static readonly pb::FieldCodec<string> _repeated_unsubscribeTopics_codec
        = pb::FieldCodec.ForString(26);
    private readonly pbc::RepeatedField<string> unsubscribeTopics_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> UnsubscribeTopics {
      get { return unsubscribeTopics_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as TopicSubscription);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(TopicSubscription other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ClientId != other.ClientId) return false;
      if(!subscribeTopics_.Equals(other.subscribeTopics_)) return false;
      if(!unsubscribeTopics_.Equals(other.unsubscribeTopics_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ClientId.Length != 0) hash ^= ClientId.GetHashCode();
      hash ^= subscribeTopics_.GetHashCode();
      hash ^= unsubscribeTopics_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ClientId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(ClientId);
      }
      subscribeTopics_.WriteTo(output, _repeated_subscribeTopics_codec);
      unsubscribeTopics_.WriteTo(output, _repeated_unsubscribeTopics_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ClientId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ClientId);
      }
      size += subscribeTopics_.CalculateSize(_repeated_subscribeTopics_codec);
      size += unsubscribeTopics_.CalculateSize(_repeated_unsubscribeTopics_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(TopicSubscription other) {
      if (other == null) {
        return;
      }
      if (other.ClientId.Length != 0) {
        ClientId = other.ClientId;
      }
      subscribeTopics_.Add(other.subscribeTopics_);
      unsubscribeTopics_.Add(other.unsubscribeTopics_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            ClientId = input.ReadString();
            break;
          }
          case 18: {
            subscribeTopics_.AddEntriesFrom(input, _repeated_subscribeTopics_codec);
            break;
          }
          case 26: {
            unsubscribeTopics_.AddEntriesFrom(input, _repeated_unsubscribeTopics_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
