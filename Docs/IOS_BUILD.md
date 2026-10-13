# Cloudora iOS development build

This is a port of Android baseline a18a3e4, not a new game or a release-approved product. Use Unity 6000.5.9f1 (b57deb96f08d), macOS, iOS Build Support and Xcode. See IOS_ARCHITECTURE.md for preserved behavior and known limitations; IOS_DEVICE_QA.md for acceptance.

## Setup
1. Unity Hub → Installs → 6000.5.9f1 → gear/Manage → Add modules → iOS Build Support → Install (already detected on the audit host).
2. Unity Hub → Projects → Add → Add project from disk → select the sibling `cloudora-ios` root. Never select cloudora-android for port work.
3. Open Assets/_Cloudora/Scenes/02_Gameplay.unity. Allow package import; investigate any Console errors before building.
4. File → Build Profiles → iOS → Switch Platform. Use Cloudora → Build → Configure iOS Development to persist settings without exporting.

## Reproducible commands
From the iOS root, with other Unity sessions for this project closed:

```sh
./Build-iOS.sh configure
./Build-iOS.sh test
./Build-iOS.sh validate
./Build-iOS.sh build
```

The test command intentionally omits -quit so the test runner can finish and produce fresh XML. Test XML and logs live in ignored Logs/. Validation requires the explicit 10,000/zero-failures marker. Build requires both successful Unity exit and the build success marker. CLOUDORA_UNITY_PATH can override the executable; CLOUDORA_IOS_BUILD_NUMBER defaults to 1 and must be a positive integer. DEVELOPER_DIR defaults to /Applications/Xcode.app/Contents/Developer, avoiding any machine-wide xcode-select change. Menu equivalent: Cloudora → Build → iOS Development Xcode Project.

Output: Builds/iOS/Cloudora/Unity-iPhone.xcodeproj. Generated output is ignored. Existing nonempty output is rejected: move it aside yourself to preserve local signing and Xcode edits, then regenerate. Unity export and Xcode native compilation are separate checkpoints.

## Configuration and rationale
Bundle com.yigityvz.cloudora; marketing version 0.1.0; build 1 by default; device SDK, iPhone, portrait, IL2CPP ARM64, Metal, low managed stripping, hidden status bar, no exit on suspend, no background mode requested. Minimum iOS 15.0 preserves the existing source PlayerSettings baseline; it is a development recommendation, not a claim that all supported hardware has passed. Do not lower it without confirming this editor/Xcode combination and device matrix. Targeting iPad is a separate layout acceptance step. Unity's existing default launch-screen generation is retained; final branded launch screen remains open. The existing 512px icon is a development placeholder; final 1024px opaque App Store art is required later.

Official versioned build guide: https://docs.unity.com/en-us/engine/6000.5/manual/platform-specific/iphone/ios-building-and-delivering . Check current Apple submission requirements separately before App Store work; this pipeline exports development only.

Android build menu implementation is compiled only under UNITY_ANDROID. Manifest XML filtering remains available to shared tests; its Android callback/interface is conditionally compiled. Android build scripts/history remain as provenance, not the iOS workflow. Shared runtime services are preserved. Native iOS warning haptic is isolated behind IHapticFeedback and only called on invalid moves with Haptics enabled. No SDK, real ad, Firebase configuration, tracking permission, certificate or Team ID is added.

## Xcode and iPhone — owner steps
1. Open Xcode; complete any first-launch component installation and licence prompts.
2. Xcode → Settings → Accounts → + → Apple Account → sign in locally. Never paste passwords or credentials into chat/Git.
3. File → Open → Builds/iOS/Cloudora/Unity-iPhone.xcodeproj.
4. Select the blue Unity-iPhone project → TARGETS → Unity-iPhone → Signing & Capabilities → enable Automatically manage signing → Team → choose your own team. Confirm Bundle Identifier com.yigityvz.cloudora. Do not commit the generated project.
5. Connect an iPhone with USB, unlock it and accept Trust This Computer → Trust. On iPhone: Settings → Privacy & Security → Developer Mode → enable → restart and confirm when requested.
6. Xcode → Window → Devices and Simulators: confirm pairing and device readiness. Select the physical iPhone in the top run-destination selector, scheme Unity-iPhone, then Product → Run (⌘R).
7. If the phone asks for developer trust: Settings → General → VPN & Device Management → select the development identity → Trust (only if prompted).
8. Capture exact build/runtime errors without secrets. A signing/provisioning issue must be resolved locally; do not change the bundle identifier as a workaround without owner discussion.

Unsigned native validation, when export exists:

```sh
DEVELOPER_DIR=/Applications/Xcode.app/Contents/Developer xcodebuild \
  -project Builds/iOS/Cloudora/Unity-iPhone.xcodeproj -scheme Unity-iPhone \
  -configuration Debug -sdk iphoneos -destination 'generic/platform=iOS' \
  -derivedDataPath Builds/iOS/DerivedData CODE_SIGNING_ALLOWED=NO build
```

An unsigned build cannot install on a physical phone and does not validate signing or gameplay. Do not archive/upload/publish as part of this development port.

## Sandbox troubleshooting
If the automated editor cannot reach its IL Post Processor socket, no test/build pass is established. Close only the affected batch Unity process in Activity Monitor and run the commands above from Terminal outside the Codex sandbox. Do not delete a UnityLockfile while the owning process is alive. Retain Logs/ios-test.log and Logs/ios-editmode.xml for inspection.
