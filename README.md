# Cloudora iOS — Android v3.2 port

This repository ports `yigityvz/cloudora-android`, branch `cloudora-bahadir`, commit `d7c9416`, to iOS. Runtime scripts, scenes, level data, tests, resource configuration and art are preserved from that source. No Figma/Stitch design proposals or gameplay/economy changes are applied.

Open with Unity **6000.5.9f1** plus iOS Build Support. Run `./Build-iOS.sh test`, then `./Build-iOS.sh build`; open `Builds/iOS/Cloudora/Unity-iPhone.xcodeproj` in Xcode. Local signing is configured by the owner. See [iOS build instructions](Docs/IOS_BUILD.md) and [current verification status](Docs/IOS_STATUS.md). Android build documentation below is retained as upstream history.

---

## Contributors

- [@yigityvz](https://github.com/yigityvz) – original author of the project
- [@bahadir-aktas](https://github.com/bahadir-aktas) – contributor (Unity development and Android builds)

Team repository: [yigityvz/cloudora-android](https://github.com/yigityvz/cloudora-android) · Bahadır's line: [`cloudora-bahadir`](https://github.com/yigityvz/cloudora-android/tree/cloudora-bahadir), mirrored at [bahadir-aktas/cloudora-android](https://github.com/bahadir-aktas/cloudora-android)

# Cloudora

**Fix the sky. Restore the world.** Cloudora is an Android-first, portrait, cozy weather-sorting puzzle built in Unity 6000.5.9f1 (Universal 2D). Tap a source cloud and then a compatible destination; matching top weather moves as a group. No level countdown or drag gesture is required.

![Cloudora feature art](StoreAssets/Play/cloudora-feature-1024x500.png)

## Product and architecture

- Fifteen authored onboarding levels lead into seed-reproducible generated puzzles. Since v3, levels 1–100 use boards picked by an editor-only solver tool to follow a fixed difficulty pattern (Medium, Medium, Hard, Medium, Very hard per five levels), with one new element at a time (weather type, capacity, single empty cloud, cloud layout) and trap levels that grow more frequent; see [the level map](Docs/V3_BOLUM_HARITASI.html). Levels 101+ still come from the generator, which reverse-shuffles solved boards and records a legal solution, then validates invariants before accepting a level.
- Since v3.1, Shuffle reorders a single chosen cloud (unlocks at level 16), Restart is removed, and running out of moves fails the level: retry for a life, or continue via ad or coins from the last solvable state.
- Cloud capacity progresses from four to seven slots. Thirteen world definitions span levels 1–400, with restoration milestones, new weather, modifiers, and difficulty waves.
- Rule logic lives in pure C# state and move classes; Unity cloud views, animations, responsive board layout, HUD, world screen, and settings sit around that core. Modifier rules are registered independently of the views.
- Local JSON save data includes progress, life regeneration, tutorial flags, settings, boosters, coins and a failed-level snapshot, with schema migration and backup recovery. No account, backend, or cloud-save service is required for the puzzle loop.
- Analytics and advertising are behind interfaces. This repository binds fake ads and a credential-free analytics fallback. Real AdMob is not enabled; no public release is authorized.

## Run and test

1. Open the project in Unity 6000.5.9f1 and load `Assets/_Cloudora/Scenes/02_Gameplay.unity`.
2. Enter Play Mode to inspect the initial menu, one-finger puzzle loop, world and settings surfaces.
3. Run EditMode tests in Unity Test Runner. `Cloudora/Validate 10,000 Levels` runs the deterministic content stress check.
4. Use `Cloudora/Build/Android Development APK` for an internal build. See [Android build and device QA](Docs/ANDROID_BUILD.md) for the ASCII-only path requirement and open physical-device checks.

The latest EditMode result (v3.2) passed 192 tests (4 skipped: three range checks that do not apply to solver-built parts and one manual measurement); an earlier separate content stress check passed 10,000/10,000 generated-level validations. These checks do not substitute for Android PlayMode/device smoke tests or prove real-world difficulty quality. See [phase progress](Docs/PROJECT_PROGRESS.md) and the [release checklist](Docs/RELEASE_CHECKLIST.md) for exact status and blockers.

## Store/portfolio package

The [store-art pack](StoreAssets/README.md) contains an original icon and feature illustration, plus exact-size PNG candidates; [English and Turkish store copy](StoreAssets/STORE_COPY.md) is a draft. These are not gameplay screenshots. Authentic Android screenshots, release signing, privacy-policy approval, and publication remain separate owner-controlled gates. See the [privacy release review](Docs/PRIVACY_RELEASE_REVIEW.md).
