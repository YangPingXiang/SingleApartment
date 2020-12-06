using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.Models
{
    public class CInformationFactory
    {
        public bool Add(int p_member_id, int p_categoryid, int p_source_id, int? p_content_id)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                Information info = new Information();

                info.InformationDate = DateTime.Now;
                info.InformationCategoryID = p_categoryid;  //訊息分類來源
                info.DocumentID = p_source_id;              //可能是訂單號碼, 房號 ......

                if (p_content_id != null)
                {
                    InformationContent rowContent = db.InformationContent.Where(c => c.ContentID == p_content_id).FirstOrDefault();
                    if (rowContent != null)
                    {
                        //InformationSource此欄位可以是null
                        info.InformationSource = p_content_id;              //訊息ContentID
                        info.InformationContent = rowContent.ContentText;   //訊息內容
                    }
                    else
                        info.InformationContent = "基本資料未輸入, 請洽系統管理員";
                }
                else
                {
                    info.InformationContent = "Empty Message!";
                }

                info.MemberID = p_member_id;
                info.Priority = "Normal";
                info.Read_YN = "N";
                info.Status = "Open";
                db.Information.Add(info);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(int p_member_id, int p_categoryid, int p_source_id, int? p_content_id, string p_message)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                Information info = new Information();

                info.InformationDate = DateTime.Now;
                info.InformationCategoryID = p_categoryid;  //訊息分類來源
                info.DocumentID = p_source_id;              //可能是訂單號碼, 房號 ......

                if (p_content_id != null)
                {
                    InformationContent rowContent = db.InformationContent.Where(c => c.ContentID == p_content_id).FirstOrDefault();
                    if (rowContent != null)
                    {
                        //InformationSource此欄位可以是null
                        info.InformationSource = p_content_id;              //訊息ContentID

                        if (string.IsNullOrEmpty(p_message))
                        {
                            info.InformationContent = rowContent.ContentText + p_message;
                        }
                        else
                            info.InformationContent = rowContent.ContentText;   //訊息內容
                    }
                    else
                        info.InformationContent = "基本資料未輸入, 請洽系統管理員";
                }
                else
                {
                    info.InformationContent = "Empty Message!";
                }

                info.MemberID = p_member_id;
                info.Priority = "Normal";
                info.Read_YN = "N";
                info.Status = "Open";
                db.Information.Add(info);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}