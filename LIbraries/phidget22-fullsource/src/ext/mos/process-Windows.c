#include <process.h>

#include "mos_basic.h"

MOSAPI int MOSCConv
mos_getprocessid(void) {

	return (_getpid());
}
