# Cloudora for iOS

**Fix the sky. Restore the world.** Portrait weather-sorting puzzle ported from the complete Cloudora Android project. Existing gameplay, first 15 authored levels, deterministic generated levels, worlds/restoration, modifiers, lives, boosters, save schema and runtime visuals are retained.

Unity **6000.5.9f1** (Universal 2D). Open this `cloudora-ios` root in Unity Hub, load `Assets/_Cloudora/Scenes/02_Gameplay.unity`, then use **Cloudora → Build → iOS Development Xcode Project**. Open the generated `Builds/iOS/Cloudora/Unity-iPhone.xcodeproj` in Xcode, select your local Team under Signing & Capabilities and run on a paired development iPhone.

- [Build/setup/signing instructions](Docs/IOS_BUILD.md)
- [Physical iPhone QA and open release gates](Docs/IOS_DEVICE_QA.md)
- [Architecture and preserved baseline limitations](Docs/IOS_ARCHITECTURE.md)
- [Fresh validation status](Docs/IOS_STATUS.md)
- [Original Android README](Docs/ANDROID_BASELINE_README.md) and [historical progress](Docs/PROJECT_PROGRESS.md)

Run `./Build-iOS.sh test` for the original EditMode suite plus iOS regression coverage; `./Build-iOS.sh validate` for the original 10,000-level validation. `./Build-iOS.sh configure` persists iOS settings; `./Build-iOS.sh build` exports a development Xcode project. Close other Unity instances for this project first. Generated files, logs and signing credentials are excluded from Git.

Android baseline commit `a18a3e49d235e12f452e2a3467b473759d096393` and history are retained. The sibling Android reference remains unchanged. This repo's only push origin is https://github.com/yigityvz/cloudora-ios.git.

Development uses fake ads; non-development uses an unavailable AdMob stub. No real Firebase/AdMob configuration is added. Device QA, final store art/launch screen, Apple privacy review, signing and App Store publication remain open. Read the current status before interpreting any historical Android PASS as iOS evidence.
