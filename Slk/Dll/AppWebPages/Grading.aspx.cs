/* Copyright (c) Microsoft Corporation. All rights reserved. */

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using Microsoft.SharePointLearningKit.WebControls;
using System.Text;
using Microsoft.SharePointLearningKit;
using Microsoft.LearningComponents.SharePoint;
using Microsoft.LearningComponents.Storage;
using Microsoft.LearningComponents;
using System.Security.Principal;
using Microsoft.LearningComponents.Manifest;
using System.Globalization;
using System.Collections.ObjectModel;
using Resources.Properties;
using System.Threading;
using System.IO;

namespace Microsoft.SharePointLearningKit.ApplicationPages
{
	/// <summary>
	/// Code-behind for Grading.aspx
	/// </summary>
	public class Grading : SlkAppBasePage
	{
		#region Control Declarations
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblPointsValue;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblStartValue;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblDueValue;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblPoints;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblStart;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblDue;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblAutoReturn;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblAnswers;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblGradeAssignment;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblGradeAssignmentDescription;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblTitle;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl")]
		protected Label lblDescription;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn")]
		protected Button btnTopSave;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn")]
		protected Button btnTopClose;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn")]
		protected Button btnBottomSave;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn")]
		protected Button btnBottomClose;

		protected SlkButton slkButtonEdit;
		protected SlkButton slkButtonCollect;
		protected SlkButton slkButtonReturn;
		protected SlkButton slkButtonDelete;

		protected Literal pageTitle;
		protected Literal pageTitleInTitlePage;
		protected Literal pageDescription;

		protected ErrorBanner errorBanner;
		protected GradingList gradingList;
		protected Panel contentPanel;
		protected Image infoImageAutoReturn;
		protected Image infoImageAnswers;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tbl")]
		protected HtmlTable tblAutoReturn;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tbl")]
		protected HtmlTable tblAnswers;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tgr")]
		protected TableGridRow tgrAutoReturn;

		#endregion

		#region Private Variables
		/// <summary>
		/// Holds assignmentID value.
		/// </summary>
		private long? m_assignmentId;
		/// <summary>
		/// Holds Assignment Properties.
		/// </summary>
		private AssignmentProperties m_assignmentProperties;
		/// <summary>
		/// Keeps track if there was an error during one of the click events.
		/// </summary>
		private bool pageHasErrors;

		#endregion

		#region Private Properties
		/// <summary>
		/// Gets the AssignmentId Query String value.
		/// </summary>
		private long? AssignmentId
		{
			get
			{
				if (m_assignmentId == null)
				{
					long id;
					QueryString.Parse(QueryStringKeys.AssignmentId, out id, false);
					m_assignmentId = id;
				}
				return m_assignmentId;
			}
		}
		/// <summary>
		/// Gets the AssignmentItemIdentifier value.
		/// </summary>
		private AssignmentItemIdentifier AssignmentItemIdentifier
		{
			get
			{
				AssignmentItemIdentifier assignmentItemId = null;
				if (AssignmentId != null)
				{
					assignmentItemId = new AssignmentItemIdentifier(AssignmentId.Value);
				}
				return assignmentItemId;
			}
		}
		/// <summary>
		/// Gets the AssignmentProperties.
		/// </summary>
		private AssignmentProperties AssignmentProperties
		{
			get
			{
				return m_assignmentProperties;
			}
		}
		#endregion

		#region OnPreRender
		/// <summary>
		///  Over rides OnPreRender.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected override void OnPreRender(EventArgs e)
		{
			try
			{
				SetResourceText();

				if (!pageHasErrors)
				{
					LoadGradingList();
                    // for the assignment site, if the user doesn't have permission to view it
                    // we'll catch the exception 
                    bool previousValue = SPSecurity.CatchAccessDeniedException;
                    SPSecurity.CatchAccessDeniedException = false;
                    try
                    {
                        using (SPSite spSite = new SPSite(AssignmentProperties.SPSiteGuid))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb(AssignmentProperties.SPWebGuid))
                            {
                                // If the assignment is in a different redirect to it.
                                if (SPWeb.ID != spWeb.ID)
                                    Response.Redirect(SlkUtilities.UrlCombine(spWeb.Url, Request.Path + "?" + Request.QueryString.ToString()));
                            }
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //do nothing. Catch the exception to not to restrict the user to access the content if user does not have access to SPWeb.
                    }
                    catch (FileNotFoundException)
                    {
                        // do nothing . Catch the exception to not to restrict the user to access the content if SPWeb does not exist.
                    }
                    finally
                    {
                        SPSecurity.CatchAccessDeniedException = previousValue;
                    }
                    
					AddReactivationCheck();

					lblTitle.Text = Server.HtmlEncode(AssignmentProperties.Title);
					lblDescription.Text = Server.HtmlEncode(AssignmentProperties.Description).Replace("\r\n", "<br />\r\n");

					if (AssignmentProperties.PointsPossible.HasValue)
						lblPointsValue.Text = AssignmentProperties.PointsPossible.Value.ToString(Constants.RoundTrip, NumberFormatInfo);
					lblStartValue.Text = string.Format(CultureInfo.CurrentCulture, AppResources.SlkDateFormatSpecifier,
												AssignmentProperties.StartDate);

					if (AssignmentProperties.DueDate.HasValue)
						lblDueValue.Text = string.Format(CultureInfo.CurrentCulture, AppResources.SlkDateFormatSpecifier,
													AssignmentProperties.DueDate.Value);
					tblAutoReturn.Visible = AssignmentProperties.AutoReturn;
					tblAnswers.Visible = AssignmentProperties.ShowAnswersToLearners;
					tgrAutoReturn.Visible = tblAutoReturn.Visible || tblAnswers.Visible;
				}
			}
			catch (ThreadAbortException)
			{
				// make sure this exception isn't caught
				throw;
			}
			catch (Exception ex)
			{
				contentPanel.Visible = false;
				errorBanner.AddException(ex);
			}
		}
		#endregion

		/// <summary>
		/// Adds in code necessary to show a warning if the user is trying to reactivate an assignment.
		/// </summary>
		private void AddReactivationCheck()
		{
			StringBuilder script = new StringBuilder();
			script.AppendLine("function CheckReactivate()");
			script.AppendLine("{");
			script.AppendLine("\tvar showMessage = false;");
			script.AppendLine("\tfor (i=0;i<document.forms[0].elements.length;i++)");
			script.AppendLine("\t{");
			script.AppendLine("\t\tvar obj = document.forms[0].elements[i];");
			script.Append("\t\tif (obj.type == \"checkbox\" && obj.checked && obj.id.indexOf(\"chkAction\") > 0 && obj.parentElement.children[1].innerText == \"");
			script.Append(AppResources.GradingActionTextFinal);
			script.AppendLine("\")");
			script.AppendLine("\t\t{");
			script.AppendLine("\t\t\tshowMessage = true;");
			script.AppendLine("\t\t}");
			script.AppendLine("\t}");
			script.AppendLine("\tif (showMessage)");
			script.Append("\t\treturn confirm(\"");
			script.AppendFormat(CultureInfo.CurrentCulture,"{0}", AppResources.GradingReactivateMessage);
			script.AppendLine("\");");
			script.AppendLine("\telse");
			script.AppendLine("\t\treturn true;");
			script.AppendLine("}");
			ClientScript.RegisterClientScriptBlock(this.GetType(), "ReactivationCheck", script.ToString(), true);

			slkButtonEdit.OnClientClick = "return CheckReactivate();";
			btnTopSave.OnClientClick = "return CheckReactivate();";
			btnBottomSave.OnClientClick = "return CheckReactivate();";
		}

		/// <summary>
		/// Event handler for the OK button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				SaveGradingList(SaveAction.SaveOnly);
				// Make the page safe to refresh
				Response.Redirect(Request.RawUrl, true);
			}
			catch (ThreadAbortException)
			{
				// Calling Response.Redirect throws a ThreadAbortException which will
				// flag an error in the next step if we don't do this.
				throw;
			}
			catch (Exception ex)
			{
				pageHasErrors = true;
				errorBanner.AddException(ex);
			}
		}
		/// <summary>
		/// Event handler for the Cancel button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
		protected void btnClose_Click(object sender, EventArgs e)
		{
			Response.Redirect(SPWeb.ServerRelativeUrl, true);
		}

		/// <summary>
		/// Event handler for the Edit button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected void slkButtonEdit_Click(object sender, EventArgs e)
		{
			try
			{
				SaveGradingList(SaveAction.SaveOnly);

				string url = SlkUtilities.UrlCombine(SPWeb.ServerRelativeUrl, "_layouts/SharePointLearningKit/AssignmentProperties.aspx");
				url = String.Format(CultureInfo.InvariantCulture, "{0}?AssignmentId={1}", url, AssignmentId.ToString());

				Response.Redirect(url, true);
			}
			catch (ThreadAbortException)
			{
				// Calling Response.Redirect throws a ThreadAbortException which will
				// flag an error in the next step if we don't do this.
				throw;
			}
			catch (Exception ex)
			{
				pageHasErrors = true;
				errorBanner.AddException(ex);
			}
		}
		/// <summary>
		/// Event handler for the Collect button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected void slkButtonCollect_Click(object sender, EventArgs e)
		{
			try
			{
				SaveGradingList(SaveAction.CollectAll);
				// Make the page safe to refresh
				Response.Redirect(Request.RawUrl, true);
			}
			catch (ThreadAbortException)
			{
				// Calling Response.Redirect throws a ThreadAbortException which will
				// flag an error in the next step if we don't do this.
				throw;
			}
			catch (Exception ex)
			{
				pageHasErrors = true;
				errorBanner.AddException(ex);
			}
		}
		/// <summary>
		/// Event handler for the Return button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected void slkButtonReturn_Click(object sender, EventArgs e)
		{
			try
			{
				SaveGradingList(SaveAction.ReturnAll);
				// Make the page safe to refresh
				Response.Redirect(Request.RawUrl, true);
			}
			catch (ThreadAbortException)
			{
				// Calling Response.Redirect throws a ThreadAbortException which will
				// flag an error in the next step if we don't do this.
				throw;
			}
			catch (Exception ex)
			{
				pageHasErrors = true;
				errorBanner.AddException(ex);
			}
		}
		/// <summary>
		/// Event handler for the Delete button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected void slkButtonDelete_Click(object sender, EventArgs e)
		{
			try
			{
				SlkStore.GetGradingProperties(AssignmentItemIdentifier, out m_assignmentProperties);
				using(SPSite spSite = new SPSite(AssignmentProperties.SPSiteGuid))
				{
				    using(SPWeb spWeb = spSite.OpenWeb(AssignmentProperties.SPWebGuid))
				    {
				        SlkStore.DeleteAssignment(AssignmentItemIdentifier);
				        Response.Redirect(spWeb.ServerRelativeUrl, true);
                    }
                }
			}
			catch (ThreadAbortException)
			{
				// Calling Response.Redirect throws a ThreadAbortException which will
				// flag an error in the next step if we don't do this.
				throw;
			}
			catch (Exception ex)
			{
				pageHasErrors = true;
				contentPanel.Visible = false;
				errorBanner.AddException(ex);
			}
		}

		#region SetResourceText
		/// <summary>
		///  Set the Control Text from Resource File.
		/// </summary>
		private void SetResourceText()
		{
			lblPoints.Text = AppResources.GradinglblPoints;
			lblStart.Text = AppResources.GradinglblStart;
			lblDue.Text = AppResources.GradinglblDue;
			lblAutoReturn.Text = AppResources.GradinglblAutoReturn;
			lblAnswers.Text = AppResources.GradinglblAnswers;
			lblGradeAssignment.Text = AppResources.GradinglblGradeAssignment;
			lblGradeAssignmentDescription.Text = AppResources.GradinglblGradeAssignmentDescription;

			pageTitle.Text = AppResources.GradingPageTitle;
			pageTitleInTitlePage.Text = AppResources.GradingPageTitleinTitlePage;
			pageDescription.Text = AppResources.GradingPageDescription;

			slkButtonEdit.Text = AppResources.GradingEditText;
			slkButtonEdit.ToolTip = AppResources.GradingEditToolTip;
			slkButtonEdit.ImageUrl = Constants.ImagePath + Constants.EditIcon;
			slkButtonEdit.AccessKey = AppResources.GradingEditAccessKey;
			slkButtonCollect.Text = AppResources.GradingCollectText;
			slkButtonCollect.ToolTip = AppResources.GradingCollectToolTip;
			slkButtonCollect.OnClientClick = String.Format(CultureInfo.CurrentCulture, "javascript: return confirm('{0}');",
				 AppResources.GradingCollectMessage);
			slkButtonCollect.ImageUrl = Constants.ImagePath + Constants.CollectAllIcon;
			slkButtonCollect.AccessKey = AppResources.GradingCollectAccessKey;
			slkButtonReturn.Text = AppResources.GradingReturnText;
			slkButtonReturn.ToolTip = AppResources.GradingReturnToolTip;
			slkButtonReturn.OnClientClick = String.Format(CultureInfo.CurrentCulture, "javascript: return confirm('{0}');",
				 AppResources.GradingReturnMessage);
			slkButtonReturn.ImageUrl = Constants.ImagePath + Constants.ReturnAllIcon;
			slkButtonReturn.AccessKey = AppResources.GradingReturnAccessKey;
			slkButtonDelete.Text = AppResources.GradingDeleteText;
			slkButtonDelete.ToolTip = AppResources.GradingDeleteToolTip;
			slkButtonDelete.OnClientClick = String.Format(CultureInfo.CurrentCulture, "javascript: return confirm('{0}');",
				 AppResources.GradingDeleteMessage);
			slkButtonDelete.ImageUrl = Constants.ImagePath + Constants.DeleteIcon;
			slkButtonDelete.AccessKey = AppResources.GradingDeleteAccessKey;

			btnTopSave.Text = AppResources.GradingSave;
			btnTopClose.Text = AppResources.GradingClose;
			btnBottomSave.Text = AppResources.GradingSave;
			btnBottomClose.Text = AppResources.GradingClose;

			infoImageAnswers.AlternateText = AppResources.SlkErrorTypeInfoToolTip;
			infoImageAutoReturn.AlternateText = AppResources.SlkErrorTypeInfoToolTip;

		}
		#endregion

		#region SaveGradingList
		/// <summary>
		/// Saves the info from the grading list
		/// </summary>
		/// <param name="action">Determines how the learner assignment statud should be handled.</param>
		private void SaveGradingList(SaveAction action)
		{
			// gradingList.DeterminePostBackGradingItems() only returns the rows that have changed
			Dictionary<string, GradingItem> gradingListItems = gradingList.DeterminePostBackGradingItems();
			List<GradingProperties> gradingPropertiesList = new List<GradingProperties>();

			if (action == SaveAction.CollectAll || action == SaveAction.ReturnAll)
			{
				foreach (GradingItem item in gradingList.Items)
				{
					switch (action)
					{
						case SaveAction.CollectAll:
							if (item.Status == LearnerAssignmentState.NotStarted || item.Status == LearnerAssignmentState.Active)
							{
								gradingListItems[item.LearnerAssignmentId.ToString(CultureInfo.InvariantCulture)] = item;
							}
							break;
						case SaveAction.ReturnAll:
							if (item.Status != LearnerAssignmentState.Final)
							{
								gradingListItems[item.LearnerAssignmentId.ToString(CultureInfo.InvariantCulture)] = item;
							}
							break;
					}
				}
			}

			foreach (GradingItem item in gradingListItems.Values)
			{
				GradingProperties gradingProperties = new GradingProperties(new LearnerAssignmentItemIdentifier(item.LearnerAssignmentId));
				gradingProperties.FinalPoints = item.FinalScore;
				gradingProperties.InstructorComments = item.InstructorComments;

                // Ignore the FinalScore Update if the Status is NotStarted or Active
                if (item.Status == LearnerAssignmentState.NotStarted || 
                    item.Status == LearnerAssignmentState.Active)
                {
                    gradingProperties.IgnoreFinalPoints = true;
                }
						

				switch (action)
				{
					case SaveAction.SaveOnly:
						// The Save or OK button was clicked
						if (item.ActionState)
						{
							switch (item.Status)
							{
								case LearnerAssignmentState.NotStarted:
									gradingProperties.Status = LearnerAssignmentState.Completed;
									break;
								case LearnerAssignmentState.Active:
									gradingProperties.Status = LearnerAssignmentState.Completed;
									break;
								case LearnerAssignmentState.Completed:
									gradingProperties.Status = LearnerAssignmentState.Final;
									break;
								case LearnerAssignmentState.Final:
									gradingProperties.Status = LearnerAssignmentState.Active;
									break;
							}
						}
						break;
					case SaveAction.CollectAll:
						// The Collect All button was clicked
						if (item.Status == LearnerAssignmentState.NotStarted || item.Status == LearnerAssignmentState.Active)
							gradingProperties.Status = LearnerAssignmentState.Completed;
						else
							gradingProperties.Status = item.Status;
						break;
					case SaveAction.ReturnAll:
						// The Return All button was clicked
						gradingProperties.Status = LearnerAssignmentState.Final;
						break;
				}
				gradingPropertiesList.Add(gradingProperties);
			}
			string warning = SlkStore.SetGradingProperties(AssignmentItemIdentifier, gradingPropertiesList);
			if (!string.IsNullOrEmpty(warning))
				errorBanner.AddError(ErrorType.Warning, warning);
		}
		#endregion

		#region LoadGradingList
		/// <summary>
		/// Loads the grading list control
		/// </summary>
		private void LoadGradingList()
		{
			try
			{
				ReadOnlyCollection<GradingProperties> itemCollection = SlkStore.GetGradingProperties(AssignmentItemIdentifier, out m_assignmentProperties);

				gradingList.IsClassServerContent = AssignmentProperties.PackageFormat.HasValue && AssignmentProperties.PackageFormat.Value == PackageFormat.Lrm;
				gradingList.Clear();
				foreach (GradingProperties item in itemCollection)
				{
					gradingList.Add(item);
				}
			}
			catch (InvalidOperationException)
			{
				throw new SafeToDisplayException(AppResources.GradingInvalidAssignmentId, AssignmentId);
			}
		}
		#endregion

		private enum SaveAction
		{
			SaveOnly,
			CollectAll,
			ReturnAll
		}
	}
}
