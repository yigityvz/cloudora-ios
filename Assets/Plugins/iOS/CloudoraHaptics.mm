#import <UIKit/UIKit.h>
extern "C" void CloudoraInvalidMoveHaptic()
{
    // Unity calls feedback from the main thread. iOS silently ignores unsupported hardware.
    UINotificationFeedbackGenerator *feedback = [[UINotificationFeedbackGenerator alloc] init];
    [feedback prepare];
    [feedback notificationOccurred:UINotificationFeedbackTypeWarning];
}
