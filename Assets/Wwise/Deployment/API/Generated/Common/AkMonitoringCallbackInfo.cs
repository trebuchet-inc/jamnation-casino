#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.11
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

public class AkMonitoringCallbackInfo : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal AkMonitoringCallbackInfo(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(AkMonitoringCallbackInfo obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~AkMonitoringCallbackInfo() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          AkSoundEnginePINVOKE.CSharp_delete_AkMonitoringCallbackInfo(swigCPtr);
        }
        swigCPtr = IntPtr.Zero;
      }
      GC.SuppressFinalize(this);
    }
  }

  public akErrorCode errorCode {
    get {
      akErrorCode ret = (akErrorCode)AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_errorCode_get(swigCPtr);

      return ret;
    } 
  }

  public ErrorLevel errorLevel {
    get {
      ErrorLevel ret = (ErrorLevel)AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_errorLevel_get(swigCPtr);

      return ret;
    } 
  }

  public uint playingID {
    get {
      uint ret = AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_playingID_get(swigCPtr);

      return ret;
    } 
  }

  public ulong gameObjID { get { return AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_gameObjID_get(swigCPtr);
 } 
  }

  public string message { get { return AkSoundEngine.StringFromIntPtrOSString(AkSoundEnginePINVOKE.CSharp_AkMonitoringCallbackInfo_message_get(swigCPtr));
 } 
  }

  public AkMonitoringCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkMonitoringCallbackInfo(), true) {

  }

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.