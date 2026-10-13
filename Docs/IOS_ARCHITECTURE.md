# iOS port audit — 2026-10-13

Baseline: Android `codex/phase2-15`, `a18a3e49d235e12f452e2a3467b473759d096393`. Full history retained; Android reference checkout is unchanged. iOS origin is https://github.com/yigityvz/cloudora-ios.git.

## Flow and boundaries
`MobileRuntimeBootstrap` sets 60 FPS, disables vSync and repairs invisible root canvases. The gameplay scene contains camera, URP 2D light, Canvas/BoardRoot, Input System EventSystem, and PuzzleController with CloudContainer prefab references. `PuzzleController.Start` loads JSON, binds Firebase fallback analytics and development-only fake ads, constructs progression/lives/boosters/modifiers, creates HUD/world/menu, and loads a level.

Tap source then destination; matching top elements move together up to free capacity. Animation locks input. Views implement live moves and solved detection; Core independently implements state, maximum-group moves and solved checks. Undo snapshots include modifier counters. Extra Cloud clears undo and preserves modifier state; Safe Shuffle replaces generated boards with validated seeded boards. Restart charges lives only after moves, with grace on levels 1–5. Completion saves the next resume level while preserving the current completion screen.

Core state/rules, generator/validator, authored catalog, life/booster/progression/restoration calculations are reusable C# (WorldDefinition uses Unity colors). Fifteen authored levels precede reverse-shuffled generated boards with known solutions. Capacities rise 4→7; 13 worlds span 1–400, generation continues beyond 400. Preserve existing world/generator metadata differences rather than rebalance during port.

Modifiers are registered behind ICloudModifierRule but depend on CloudContainerView: Frozen blocks sourcing for two successful moves (receiving remains allowed); Locked gates both directions on solved key cloud; Fog reveals after interaction; Rainbow allows wildcard receive/solve; Night hides lower contents; Wind limits outgoing transfer to one token. Known-solution validation uses core rules, not a full modifier-aware solver: 10,000 validation is valuable but does not prove every live modifier interaction.

## Persistence and services
Schema 2 saves progress, tutorial flags, UTC regeneration timestamp, balances, settings and last generated seed. It does NOT serialize in-progress board, undo history or shuffled-board resume. LoadLevel regenerates the default seed; lastGeneratedSeed is recorded but not consumed on resume. This baseline behavior is preserved. Save uses temp + copy + backup, not an atomic file replacement. Pause and quit save; iOS quit callbacks are not guaranteed, so pause is essential. UTC life math avoids timezone dependence; clock rollback/forward remains device QA.

Firebase is reflection-discovered, logs event names if present, always mirrors fallback parameters; no SDK credentials included. Debug builds bind FakeAdService; release binds unavailable AdMob stub. Do not add real ads/Firebase or network consent flows. Feedback audio clips have no runtime assignments in this source; Sound toggle exists but adding new audio is a separate product change. Existing haptic is Handheld.Vibrate on invalid moves only.

## Assets and configuration
Reviewed all six Docs files, README, StoreAssets docs/export script, C# sources/tests, gameplay scene/prefab, package manifest/lock, PlayerSettings/build/graphics/input configuration and repository inventory. _Cloudora has Scripts, Editor, Scenes, Prefabs, Tests and Art; no populated separate UI/Audio/Materials/ScriptableObjects asset folders. UI lives in Scripts/UI and mostly generates visuals at runtime. Settings holds URP renderer/assets and input actions. Sample/template scenes are not build scenes.

Direct packages: URP 17.6.0, Input System 1.20.0, Test Framework 1.7.0, uGUI 2.5.0; 2D animation 15.1.0, Aseprite 5.0.3, PSD 14.0.3, Sprite 1.0.0, SpriteShape 15.0.3, Tilemap 1.0.0, extras 8.0.3, tooling 3.0.1; AI navigation 2.0.14, collab 2.13.6, Rider 3.0.38, VS 2.0.26, Timeline 1.8.12, Visual Scripting 1.9.12, XR helpers 3.0.1 and engine modules. Keep versions during port. Engine analytics/network modules alone are not evidence of live collection or absence of collection.

## Android inventory and iOS risks
Android-specific code: CloudoraBuildTools (IL2CPP ARM64 API26/36 APK/AAB, debug/release/Play signing preflight, adaptive icons/public symbols), manifest postprocessor (INTERNET removal), Windows Build-Android.ps1 TEMP workaround, Android build profile, icon importer overrides and PlayerSettings. No AndroidJavaObject/Class or native runtime plugins found. Isolate Android editor API imports/callbacks while retaining XML filter and its regression test on every editor platform.

Shared InputSystemUIInputModule, CanvasScaler 1080×1920, SafeAreaFitter and AdaptiveBoardLayout can carry over. Risks: fixed HUD offsets/widths, safe-area changes without rect-size change, smallest screens, glyph fallback, Dynamic Island/home indicator, animation interruption, JSON copy interruption, IL2CPP stripping/reflection and native haptic linking. Preserve graphics, accessibility patterns and all original tests. Screen equations alone are not physical UI acceptance.

## Phases and evidence policy
0 audit; 1 historical baseline; 2 editor compilation/isolation; 3 iOS settings/build wrapper; 4 isolated haptics; 5 layout/lifecycle QA; 6 original + focused regression and 10,000 validation; 7 Xcode generation; 8 owner signing/physical iPhone; 9 QA/privacy; 10 App Store later. Record PASS/FAIL/NOT RUN/BLOCKED/MANUAL TEST REQUIRED from fresh evidence only. Android historical results are not current iOS test results.
