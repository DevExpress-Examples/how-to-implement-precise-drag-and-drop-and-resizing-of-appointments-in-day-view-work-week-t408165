using System;
using DevExpress.XtraScheduler;

public class AppointmentDragInfo {
    public AppointmentDragInfo() { }

    public Appointment Appointment { get; set; }
    public TimeSpan TimeAtCursor { get; set; }

    public TimeInterval InitialInterval { get; set; }
}