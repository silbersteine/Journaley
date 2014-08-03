﻿namespace Journaley.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// The base form class for providing custom title bar dragging capability.
    /// </summary>
    public partial class BaseJournaleyForm : Form
    {
        /// <summary>
        /// Indicates whether the title bar is being dragged.
        /// </summary>
        private bool draggingTitleBar = false;

        /// <summary>
        /// The dragging offset
        /// </summary>
        private Point draggingOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJournaleyForm"/> class.
        /// </summary>
        public BaseJournaleyForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether [dragging title bar].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [dragging title bar]; otherwise, <c>false</c>.
        /// </value>
        private bool DraggingTitleBar
        {
            get
            {
                return this.draggingTitleBar;
            }

            set
            {
                if (this.draggingTitleBar == true && value == false)
                {
                    if (this.WindowState != FormWindowState.Maximized && this.Location.Y < 0)
                    {
                        this.Location = new Point(this.Location.X, 0);
                    }
                }

                this.draggingTitleBar = value;
            }
        }

        /// <summary>
        /// Handles the Deactivate event of the BaseJournaleyForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BaseJournaleyForm_Deactivate(object sender, EventArgs e)
        {
            this.DraggingTitleBar = false;
        }

        /// <summary>
        /// Handles the MouseDown event of the panel title bar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void PanelTitlebar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.DraggingTitleBar = true;

                if (this.WindowState == FormWindowState.Maximized)
                {
                    Point offset = new Point();
                    if (this.RestoreBounds.Width >= this.Width)
                    {
                        offset = e.Location;
                    }
                    else if (e.X < this.RestoreBounds.Width / 2)
                    {
                        offset = e.Location;
                    }
                    else if (e.X >= this.Width - (this.RestoreBounds.Width / 2))
                    {
                        offset = new Point(e.Location.X - (this.Width - this.RestoreBounds.Width), e.Location.Y);
                    }
                    else
                    {
                        offset = new Point(this.RestoreBounds.Width / 2, e.Location.Y);
                    }

                    this.draggingOffset = offset;
                }
                else
                {
                    this.draggingOffset = e.Location;
                }
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the panel title bar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void PanelTitlebar_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.DraggingTitleBar)
            {
                // Make it normal state before moving.
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                }

                Point curScreenPos = this.PointToScreen(e.Location);
                this.Location = new Point(curScreenPos.X - this.draggingOffset.X, curScreenPos.Y - this.draggingOffset.Y);
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the panel title bar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void PanelTitlebar_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.PointToScreen(e.Location).Y == 0 && this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }

            this.DraggingTitleBar = false;
        }

        /// <summary>
        /// Handles the Click event of the imageButtonFormClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ImageButtonFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
