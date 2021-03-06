Installing SLK 

This document contains the installation and configuration instructions for SLK version 1.5. It has the following sections: 

1.  Introduction - About SharePoint Learning Kit
2.  Requirements
3.  Contents of the installation package
4.  Installing SLK
5.  Configuring SLK
6.  Upgrading from 1.4 

1.  Introduction - About SharePoint Learning Kit 

SharePoint Learning Kit (SLK) is a SCORM 2004-conformant e-learning delivery and tracking application built as a SharePoint solution. It works with either SharePoint 2010 or SharePoint 2007 and has the following core features: 

    a. Supports SCORM 1.2, SCORM 2004, and Class Server content, allowing users to store and manage this content in SharePoint document libraries.
    b. Supports a learner-centric or instructor-led (assigned) workflow.
    c. Allows assignment, tracking and grading of both e-learning and non-e-learning content. 

The current version is 1.5. 

SharePoint Learning Kit is a community source project. This means that it is made available to you for free, and that you can use it as-is or modify it to suit your needs as long as you comply with the conditions that are described in the accompanying License.txt file. You are responsible for reading and complying with this license. SharePoint Learning Kit is supported by a community of users and developers through discussion forums at http://www.codeplex.com/slk. You are invited to participate in this community, to help others, to report issues, and to contribute your ideas to carry SLK forward. For the most up to date information about SLK, please refer to http://www.codeplex.com/slk. 

2.  Requirements 

    a.  You must have a SharePoint 2007 or SharePoint 2010 environment already set up.
        i.  For SharePoint 2007, SLK will run on either Windows SharePoint Services 3 or Microsoft Office SharePoint Server 2007
        ii. For SharePoint 2010, SLK will run on SharePoint Foundation or SharePoint Server.
    b.  SLK stores its results in a SQL Server database so you need to know which SQL Server instance you want to create the database in. 

3.  Contents of the installation package 

    a.  SharePointLearningKit.wsp:  The solution package
    b.  Installation.txt: This help file
    c.  slkadm.exe: Command line configuration tool
    d.  AddSolution.ps1 or AddSolution.cmd (depending on whether for SharePoint 2007 or SharePoint 2010)
    e.  UpdateSolutionNavigation.cmd (if for SharePoint 2007)
    f.  license.txt: The license for use of SLK 
    g.  SlkSchema.sql: The database creation script. Used by slkadm.
    h.  SlkSettings.xml : The default settings file. Used by slkadm.

4.  Installing SLK 

    3.a.    For SharePoint 2010 

        i.      Unzip the installation package on a web front end server.
        ii.     Right click on AddSolution.ps1 and choose properties. Then click the unblock button.
        iii.    Add the solution to SharePoint by running AddSolution.ps1. This will add the solution to the solution gallery. Ro run AddSolution.ps1
                - Find PowerShell or SharePoint 2010 Management Shell in the start menu (All Programs | Microsoft SharePoint 2010 Programs)
                - Right Click on PowerShell or SharePoint 2010 Management Shell and choose Run as Administrator
                - cd to the directory containing the installation files
                - type in .\AddSolution.ps1 and hit enter
        iv.     Open SharePoint 2010 Central Administration
        v.      Click on System Settings
        vi.     Under Farm Management click on Manage farm solutions
        vii.    Click on sharepointlearningkit.wsp
        viii.   You now need to deploy to solution so click on Deploy Solution.
        ix.     Choose when you want to deploy the solution and which web application to deploy the solution to. This needs to be the web application which hosts the web pages/sites where your users will be using SLK for teaching & learning. Then click OK.
        x.      This will add the deploy job to the SharePoint job queue and return you to the list of installed solutions. Even if you selected to deploy immediately it will take a bit of time to deploy.

    3.b.    For SharePoint 2007
        i.      Unzip the installation package on a web front end server.
        ii.     Add the solution to SharePoint by running AddSolution.cmd. This will add the solution to the solution gallery.
        iii.    Run UpdateSolutionNavigation.cmd
        iv.     Open SharePoint 3.0 Central Administration
        v.      Click on Operations
        vi.     Under Global Configuration click on Solution Management
        vii.    Click on sharepointlearningkit.wsp
        viii.   You now need to deploy to solution so click on Deploy Solution.
        ix.     Choose when you want to deploy the solution and which web application to deploy the solution to. This needs to be the web application which hosts the web pages/sites where your users will be using SLK for teaching & learning. Then click OK.
        x.      This will add the deploy job to the SharePoint job queue and return you to the list of installed solutions. Even if you selected to deploy immediately it will take a bit of time to deploy. 

5.  Configuring SLK
    Once SLK is installed and deployed in SharePoint you need to configure it . 

    SLK is configured on a site collection basis and so you must configure SLK for each site collection in which you wish to use SLK for e-learning. Multiple site collections can share the same database. When viewing all assignments in SLK it will show them across the SLK database configured for that site collection. So if you have multiple site collections sharing an SLK database, it will show all assignments across those site collections. When you use multiple site collections in your SharePoint set up, you will need to decide if each site collection should have its own siloed SLK database or share the database depending on how you want to partition the views of assignments. There is no right or wrong way, it depends on how you have set your SharePoint portal up and your business processes. 

    The same page is used to initially set the configuration for a site collection and to update it later. The process is exactly the same. 

    When configuring SLK through central administration, the identity the application pool is running as must have appropriate permissions on the SQL Server instance being used. 

    a.  Open SharePoint Central Adminstation.
    b.  Open the SharePoint Learning Kit Configuration page which is under Application Management | SharePoint Learning Kit Configuration | Configure SharePoint Learning Kit
        If this page is not present on the page then enable the Farm level feature SharePoint Learning Kit Administration.
    c.  Choose the site collection to configure.
    d.  Choose the SQL server to use and the database name. If you are using a named instance of SQL Server remember to include the the instance name as part of the server name. You can either create a new SLK database or use an existing one.
    e.  Choose the names of the permissions used by SLK. You can use names of existing permissions, but it is likely to be easier and less confusing to use new ones. Check the Create Permissions check box if these permissions need creating in the site collection.
    f.  If you want to use non-standard setting then choose the SLK Settings file to use. It's safe to say that if you don't know what the SLK Settings file is, then you should be using the standard one.
    h.  Click OK. SharePoint will then configure SLK for the chosen site collection and report back any errors.
    i.  Navigate to the root of the site collection you have just configured.
    j.  Activate the "SharePoint Learning Kit - Assignment List Web Part". This will add the Assignment List Web Part to the web part gallery.
        i.      Select Site Actions | Site Settings to get to the site setting page.
        ii.     Under Site Collection Administration select Site Collection Features.
        iii.    Activate the feature.
    h.  On sites containing your learning resources select Site Actions | Site Settings | Mange Site Features. Activate the appropriate features chosen from:
        i.      SharePoint Learning Kit. Allowing assigning of resources to any site in the site collection.
        ii.     SharePoint Learning Kit - Assign Self. Allowing quick assigning to the logged in user.
        iii.    SharePoint Learning Kit - Assign To Site. Allowing quick assigning to learners in the current site. 

6.  Upgrading SLK 

To upgrade from 1.4 you need to: 
    a.  Retract the SharePoint Learning Kit solution
    b.  Once retraction has finished then remove the solution
    c.  Then add solution as in installing SLK and then deploy it to the appropriate web applications 

There are no database changes required.
