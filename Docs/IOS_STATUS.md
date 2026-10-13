# iOS port status — 2026-10-13

| Checkpoint | Status | Evidence |
|---|---|---|
| Source clean, reference preserved | PASS | Android baseline a18a3e4; no source edits |
| History-preserving sibling iOS/main baseline push | PASS | origin cloudora-ios; original history preserved |
| LFS history transfer/integrity | PASS | 6 historical objects uploaded to iOS; git lfs fsck OK |
| Repository/code audit | PASS | IOS_ARCHITECTURE.md |
| iOS editor compilation | PASS | Fresh Unity run after provisioning API correction; no C# warnings/errors |
| Port EditMode | PASS | 40/40 (32 original + 8 iOS), Logs/ios-editmode.xml, 2026-10-13 08:44:19–20 UTC |
| 10,000-level validation | PASS | total=10000 failures=0; difficulty 34.5–110.3, Logs/ios-validate.log |
| iOS configuration persisted by Unity | PASS | Bundle/version/build, iPhone portrait, IL2CPP ARM64, Metal, iOS 15 and no Team/profile saved |
| Xcode export | PASS | CLOUDORA_IOS_BUILD result=Succeeded, errors=0 warnings=5; Logs/ios-build.log |
| Unsigned native Xcode compilation | PASS | Xcode 27.0/iPhoneOS27.0, Debug arm64, CODE_SIGNING_ALLOWED=NO, BUILD SUCCEEDED |
| Signed native build | PASS | Owner configured local Team; BUILD SUCCEEDED, Logs/ios-signed-build.log; temporary products outside synced Documents |
| Physical iPhone installation/launch | PASS | devicectl install succeeded; running Cloudora process verified; iPhone SE (iPhone12,8) |
| Basic physical gameplay | PASS (owner report) | Owner confirmed app opened and was being played on 2026-10-13 |
| Physical save write | PASS | Read-only Documents/cloudora-save.json extraction: schema 2, current level 5, highest completed level 4 |
| Full physical acceptance matrix | MANUAL TEST REQUIRED | Undo/restart, modifiers, haptics, lifecycle/recovery, sustained performance and other screen sizes remain unverified |
| Privacy/store/release | NOT RUN | Not release-ready |

Host: Unity 6000.5.9f1, iOS module, Xcode 27.0/iOS 27 SDK. Owner completed Unity activation and Xcode licence acceptance. Early sandbox/ILPP/licence attempts produced no test results and are superseded by the fresh successful test XML above. First export detected missing LFS payload; resolved by fetching all objects from a push-disabled Android remote and uploading to iOS origin.

Package Manager reported URP requested 17.6.0 overridden by editor built-in 17.5.0 and transitive minimum overrides (searcher/Burst/performance tests). Manifest/lock were not intentionally changed. This editor behavior is documented rather than silently changing package pins; visual/device parity still requires QA. Generated Xcode, logs, caches, local IDE files and signing credentials are excluded from commits.

Export warnings: five compressed-icon warnings for the inherited 512px development icon. Native Xcode compilation warns that Unity-generated IL2CPP commands use -mios-version-min=11.0 although Xcode application deployment target is 15.0. These are retained diagnostics, not hidden by suppression. Final icon quality and actual minimum-OS device behavior remain QA gates.

Signed build initially failed because Finder metadata was reattached to generated frameworks under synced Documents. Building products under /private/tmp resolved codesigning; see IOS_BUILD.md. Installed application metadata verified bundle com.yigityvz.cloudora, version 0.1.0, build 1, minimum iOS 15.0 and iPhone device family. Device identifiers, Team details and the extracted save remain local/ignored. Save extraction confirms writes and completed-level progress, not termination recovery or upgrade persistence.
