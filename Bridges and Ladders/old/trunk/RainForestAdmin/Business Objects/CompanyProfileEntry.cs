using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Xml.Serialization;

namespace RainForestAdmin.Business_Objects
{
    [Serializable]
    [XmlRoot]
    public class CompanyProfileEntry
    {
        private string companyLogoPath;

        public string CompanyLogoPath
        {
            get { return companyLogoPath; }
            set { companyLogoPath = value; }
        }
        private string companyRegisteredName;

        public string CompanyRegisteredName
        {
            get { return companyRegisteredName; }
            set { companyRegisteredName = value; }
        }

        private string companyClass;

        public string CompanyClass
        {
            get { return companyClass; }
            set { companyClass = value; }
        }
        private string companyType;

        public string CompanyType
        {
            get { return companyType; }
            set { companyType = value; }
        }
        private string industry;

        public string Industry
        {
            get { return industry; }
            set { industry = value; }
        }
        private string numberofEmployeesWW;

        public string NumberofEmployeesWW
        {
            get { return numberofEmployeesWW; }
            set { numberofEmployeesWW = value; }
        }
        private string numberofEmployeesInIndia;

        public string NumberofEmployeesInIndia
        {
            get { return numberofEmployeesInIndia; }
            set { numberofEmployeesInIndia = value; }
        }

        private List<CompanyHeads> companyHeads;

        public List<CompanyHeads> CompanyHeads
        {
            get { return companyHeads; }
            set { companyHeads = value; }
        }
        private string companyUrl;

        public string CompanyUrl
        {
            get { return companyUrl; }
            set { companyUrl = value; }
        }
        private string compnyContactInfo;

        public string CompnyContactInfo
        {
            get { return compnyContactInfo; }
            set { compnyContactInfo = value; }
        }
        private string fortune1000;

        public string Fortune1000
        {
            get { return fortune1000; }
            set { fortune1000 = value; }
        }
        private List<String> companyLocations;

        public List<String> CompanyLocations
        {
            get { return companyLocations; }
            set { companyLocations = value; }
        }
        private List<String> companyPictures;

        public List<String> CompanyPictures
        {
            get { return companyPictures; }
            set { companyPictures = value; }
        }
        private string videoUrlEmbed;

        public string VideoUrlEmbed
        {
            get { return videoUrlEmbed; }
            set { videoUrlEmbed = value; }
        }
        private string materialsUrl;

        public string MaterialsUrl
        {
            get { return materialsUrl; }
            set { materialsUrl = value; }
        }
        private string workCulture;

        public string WorkCulture
        {
            get { return workCulture; }
            set { workCulture = value; }
        }
        private string insiderScoop;

        public string InsiderScoop
        {
            get { return insiderScoop; }
            set { insiderScoop = value; }
        }
        private string dailyExperience;

        public string DailyExperience
        {
            get { return dailyExperience; }
            set { dailyExperience = value; }
        }
        private string recruitmentProcess;

        public string RecruitmentProcess
        {
            get { return recruitmentProcess; }
            set { recruitmentProcess = value; }
        }
        private string growthPath;

        public string GrowthPath
        {
            get { return growthPath; }
            set { growthPath = value; }
        }
        private string postionAndCompensation;

        public string PostionAndCompensation
        {
            get { return postionAndCompensation; }
            set { postionAndCompensation = value; }
        }
        private string payComparision;

        public string PayComparision
        {
            get { return payComparision; }
            set { payComparision = value; }
        }
        private string otherBenefits;

        public string OtherBenefits
        {
            get { return otherBenefits; }
            set { otherBenefits = value; }
        }


    }
    [Serializable]   
    public class CompanyHeads
    {
       
        private string name;
        
        private string position;
        
        public string Name
        {
            get { return name; }
            set { name = value; }
        }       

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        
    }
}