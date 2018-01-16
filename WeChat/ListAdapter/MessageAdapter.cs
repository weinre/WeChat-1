﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Business.Model;
using WeChat.Properties;
using WinForm.UI.Controls;

namespace WeChat.ListAdapter
{
    /***
    * ===========================================================
    * 创建人：yuanj
    * 创建时间：2018/01/16 11:29:26
    * 说明：
    * ==========================================================
    * */
    public class MessageAdapter : BaseAdapter<WMessage>
    {
        private Image defaultImage;
        public MessageAdapter()
        {
            defaultImage = Resources.default_head;
        }

        public override void GetView(int position, ViewHolder holder, Graphics g)
        {
            WMessage obj = GetItem(position);
            Rectangle rec = Rectangle.Empty;
            if (obj.SendHeadImage != null)
                defaultImage = obj.SendHeadImage;
            if (obj.IsSend)
            {
                rec = new Rectangle(holder.bounds.Width - 65, holder.bounds.Y + 10, 40, 40);

            }
            else
            {
                rec = new Rectangle(20, holder.bounds.Y + 10, 40, 40);
            }
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(defaultImage, rec, new Rectangle(0, 0, defaultImage.Width, defaultImage.Height), GraphicsUnit.Pixel);
            switch (obj.MsgType)
            {
                case 1:
                    TextHolder.DrawItem(obj.Content, holder,g, obj.IsSend);
                    break;
                case 34:
                    TextHolder.DrawItem("【语音消息】MsgType="+ obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 3:
                    TextHolder.DrawItem("【图片消息】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 47:
                    ClownHolder.DrawItem(obj.FileContent,holder,g,obj.IsSend);
                    //TextHolder.DrawItem("【动画表情】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 49:
                    TextHolder.DrawItem("【红包消息/文件消息/分享连接】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 42:
                    TextHolder.DrawItem("【名片消息】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 43:
                    TextHolder.DrawItem("【小视频消息】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 37:
                    TextHolder.DrawItem("【添加好友 通知】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 8558:
                    TextHolder.DrawItem("【位置信息】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 9000:
                    TextHolder.DrawItem("【文件消息】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                case 10000:
                    TextHolder.DrawItem("【系统消息】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
                default:
                    TextHolder.DrawItem("【未知消息类型】MsgType=" + obj.MsgType, holder, g, obj.IsSend);
                    break;
            }
        }
    }
}
