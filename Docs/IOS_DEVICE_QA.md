# Physical iPhone acceptance

Status: MANUAL TEST REQUIRED for every device row until actually executed. Record date, device/iOS, commit, build number, install method, result and reproduction/log evidence. Simulator/layout math does not prove touch, haptics, audio, performance, lifecycle or signing.

## Required matrix
- Small portrait home-button iPhone, notched iPhone, Dynamic Island iPhone; cover narrow/tall aspects, home indicator and safe-area changes.
- Main menu, settings, world, HUD, level completion, no lives/moves and rewards: no clipped labels/buttons or overlap; controls remain tappable with one thumb. Confirm weather symbols are distinguishable without color, glyphs render and tutorial cues remain readable.
- Levels 1, 5, 15, 16, 45, 65, 85, 105, 155, 180, 200 and 400: source selection/cancel, grouped moves, invalid moves, completion, Continue, restart, Undo, Extra Cloud, Safe Shuffle, world boundaries/restoration. Compare against the Android baseline behavior, including modifier counters and charge deductions.
- Frozen source delay/receiving, Locked key unlock, Fog reveal, Rainbow wildcard, Night visibility and Wind one-token transfer. Undo restores modifier state; Extra Cloud preserves counters.
- Development reward is immediate FakeAdService. Non-development service is unavailable; no live ad/network integration is accepted or expected.

## Save/lifecycle matrix
Unity documents iOS persistentDataPath as the app container's Documents directory. Keeping the bundle identifier stable preserves the update path; absolute container GUIDs must never be hardcoded. Source: https://docs.unity.com/en-us/engine/6000.5/script-reference/unityengine/application/persistentdatapath . This is API-contract confirmation, not a physical-device save pass.

1. Fresh install: default progress/lives/boosters and settings; no login dependency.
2. Make moves, change settings/use boosters, background then foreground; resume same live board within the same process. Repeat during transfer animation, completion, reward and settings.
3. Background then terminate process and reopen: persisted meta-state restored; unfinished level starts from baseline board, completed level resumes at next level. Exact board/undo/shuffled seed resume is NOT implemented in Android baseline.
4. Repeat pause/termination around save writes. Corrupt primary with valid backup in a controlled test container: prior valid save recovered. Corrupt both: safe defaults. Validate legacy schema normalization and malformed timestamps. Never alter a user's only save for QA.
5. Consume lives after tutorial; wait 30 minutes, background/reopen offline; regeneration and five-life cap. Repeat across timezone/DST changes, clock rollback/forward and upgrade over a previous build. Use UTC expectations; document clock manipulation limitations.
6. Upgrade using same bundle ID/signing identity; verify retained local progress/settings. Uninstall/reinstall persistence and device backup restoration need separate observation.
7. Confirm no reliance on OnApplicationQuit on iOS; pause saves before suspension. Foreground refreshes life countdown. Test phone lock, incoming interruption and rapid app switching.

## Feedback/performance/privacy
- Haptic toggle OFF produces no warning haptic; ON produces invalid-move warning on supported hardware. Simulator is insufficient.
- Sound toggle persists; baseline has no assigned feedback clips, so silence alone is not an iOS regression. Adding sound assets is outside this port.
- Verify silent switch/interruption behavior if audio is added later. Ten sessions, a sustained session, frame pacing against 60 FPS target, memory/thermal behavior and Console errors.
- Inspect generated Info.plist, entitlements, native plugins, privacy manifests/required-reason APIs and final runtime network traffic before making any privacy claim. No accounts, real ads or Firebase SDK are configured. Existing privacy docs are historical Android context, not Apple declarations.

## Release gates — all open
Physical QA and authentic screenshots; final opaque 1024 icon and launch screen; approved support/privacy metadata; final binary privacy manifests/data declarations; signing/provisioning; store listing and content rating; App Store tooling/submission requirements; explicit owner publication approval. No current checkpoint implies release-ready.
