// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: proto/sessions/ioMapping.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ubii.Sessions {

  /// <summary>Holder for reflection information generated from proto/sessions/ioMapping.proto</summary>
  public static partial class IoMappingReflection {

    #region Descriptor
    /// <summary>File descriptor for proto/sessions/ioMapping.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static IoMappingReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch5wcm90by9zZXNzaW9ucy9pb01hcHBpbmcucHJvdG8SDXViaWkuc2Vzc2lv",
            "bnMaLHByb3RvL3Nlc3Npb25zL2ludGVyYWN0aW9uSW5wdXRNYXBwaW5nLnBy",
            "b3RvGi1wcm90by9zZXNzaW9ucy9pbnRlcmFjdGlvbk91dHB1dE1hcHBpbmcu",
            "cHJvdG8ipQEKCUlPTWFwcGluZxIWCg5pbnRlcmFjdGlvbl9pZBgBIAEoCRI+",
            "Cg5pbnB1dF9tYXBwaW5ncxgCIAMoCzImLnViaWkuc2Vzc2lvbnMuSW50ZXJh",
            "Y3Rpb25JbnB1dE1hcHBpbmcSQAoPb3V0cHV0X21hcHBpbmdzGAMgAygLMicu",
            "dWJpaS5zZXNzaW9ucy5JbnRlcmFjdGlvbk91dHB1dE1hcHBpbmciOwoNSU9N",
            "YXBwaW5nTGlzdBIqCghlbGVtZW50cxgBIAMoCzIYLnViaWkuc2Vzc2lvbnMu",
            "SU9NYXBwaW5nYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Ubii.Sessions.InteractionInputMappingReflection.Descriptor, global::Ubii.Sessions.InteractionOutputMappingReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ubii.Sessions.IOMapping), global::Ubii.Sessions.IOMapping.Parser, new[]{ "InteractionId", "InputMappings", "OutputMappings" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ubii.Sessions.IOMappingList), global::Ubii.Sessions.IOMappingList.Parser, new[]{ "Elements" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class IOMapping : pb::IMessage<IOMapping> {
    private static readonly pb::MessageParser<IOMapping> _parser = new pb::MessageParser<IOMapping>(() => new IOMapping());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<IOMapping> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ubii.Sessions.IoMappingReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IOMapping() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IOMapping(IOMapping other) : this() {
      interactionId_ = other.interactionId_;
      inputMappings_ = other.inputMappings_.Clone();
      outputMappings_ = other.outputMappings_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IOMapping Clone() {
      return new IOMapping(this);
    }

    /// <summary>Field number for the "interaction_id" field.</summary>
    public const int InteractionIdFieldNumber = 1;
    private string interactionId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string InteractionId {
      get { return interactionId_; }
      set {
        interactionId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "input_mappings" field.</summary>
    public const int InputMappingsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Ubii.Sessions.InteractionInputMapping> _repeated_inputMappings_codec
        = pb::FieldCodec.ForMessage(18, global::Ubii.Sessions.InteractionInputMapping.Parser);
    private readonly pbc::RepeatedField<global::Ubii.Sessions.InteractionInputMapping> inputMappings_ = new pbc::RepeatedField<global::Ubii.Sessions.InteractionInputMapping>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Ubii.Sessions.InteractionInputMapping> InputMappings {
      get { return inputMappings_; }
    }

    /// <summary>Field number for the "output_mappings" field.</summary>
    public const int OutputMappingsFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Ubii.Sessions.InteractionOutputMapping> _repeated_outputMappings_codec
        = pb::FieldCodec.ForMessage(26, global::Ubii.Sessions.InteractionOutputMapping.Parser);
    private readonly pbc::RepeatedField<global::Ubii.Sessions.InteractionOutputMapping> outputMappings_ = new pbc::RepeatedField<global::Ubii.Sessions.InteractionOutputMapping>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Ubii.Sessions.InteractionOutputMapping> OutputMappings {
      get { return outputMappings_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as IOMapping);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(IOMapping other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (InteractionId != other.InteractionId) return false;
      if(!inputMappings_.Equals(other.inputMappings_)) return false;
      if(!outputMappings_.Equals(other.outputMappings_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (InteractionId.Length != 0) hash ^= InteractionId.GetHashCode();
      hash ^= inputMappings_.GetHashCode();
      hash ^= outputMappings_.GetHashCode();
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
      if (InteractionId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(InteractionId);
      }
      inputMappings_.WriteTo(output, _repeated_inputMappings_codec);
      outputMappings_.WriteTo(output, _repeated_outputMappings_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (InteractionId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(InteractionId);
      }
      size += inputMappings_.CalculateSize(_repeated_inputMappings_codec);
      size += outputMappings_.CalculateSize(_repeated_outputMappings_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(IOMapping other) {
      if (other == null) {
        return;
      }
      if (other.InteractionId.Length != 0) {
        InteractionId = other.InteractionId;
      }
      inputMappings_.Add(other.inputMappings_);
      outputMappings_.Add(other.outputMappings_);
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
            InteractionId = input.ReadString();
            break;
          }
          case 18: {
            inputMappings_.AddEntriesFrom(input, _repeated_inputMappings_codec);
            break;
          }
          case 26: {
            outputMappings_.AddEntriesFrom(input, _repeated_outputMappings_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class IOMappingList : pb::IMessage<IOMappingList> {
    private static readonly pb::MessageParser<IOMappingList> _parser = new pb::MessageParser<IOMappingList>(() => new IOMappingList());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<IOMappingList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ubii.Sessions.IoMappingReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IOMappingList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IOMappingList(IOMappingList other) : this() {
      elements_ = other.elements_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public IOMappingList Clone() {
      return new IOMappingList(this);
    }

    /// <summary>Field number for the "elements" field.</summary>
    public const int ElementsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Ubii.Sessions.IOMapping> _repeated_elements_codec
        = pb::FieldCodec.ForMessage(10, global::Ubii.Sessions.IOMapping.Parser);
    private readonly pbc::RepeatedField<global::Ubii.Sessions.IOMapping> elements_ = new pbc::RepeatedField<global::Ubii.Sessions.IOMapping>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Ubii.Sessions.IOMapping> Elements {
      get { return elements_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as IOMappingList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(IOMappingList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!elements_.Equals(other.elements_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= elements_.GetHashCode();
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
      elements_.WriteTo(output, _repeated_elements_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += elements_.CalculateSize(_repeated_elements_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(IOMappingList other) {
      if (other == null) {
        return;
      }
      elements_.Add(other.elements_);
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
            elements_.AddEntriesFrom(input, _repeated_elements_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
