using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProjectTwo.Models;
using System.Diagnostics;

namespace PassionProjectTwo.Controllers
{
    public class WoWClassDataController : ApiController
    {
        private MyDbContext db = new MyDbContext();

        /// <summary>
        /// Gets a list of classes that are in the database
        /// </summary>
        /// <returns>List of classes</returns>
        // GET: api/ClassData
        [ResponseType(typeof(IEnumerable<WoWClass>))]
        public IHttpActionResult GetClasses()
        {
            List<WoWClass> Classes = db.Classes.ToList();
            List<WoWClassDto> ClassDtos = new List<WoWClassDto> { };

            //choosing information to be shown
            foreach (var Class in Classes)
            {
                WoWClassDto NewClass = new WoWClassDto
                {
                    ClassID = Class.ClassID,
                    ClassSpec = Class.ClassSpec,
                    ClassName = Class.ClassName,
                    ClassPic = Class.ClassPic
                };
                ClassDtos.Add(NewClass);
            }

            return Ok(ClassDtos);
        }

        /// <summary>
        /// Find a class in the database using its ID
        /// </summary>
        /// <param name="id">2</param>
        /// <returns>Returns the class with an id of 2</returns>

        // PUT: api/ClassData/5
        [HttpGet]
        [ResponseType(typeof(WoWClass))]
        public IHttpActionResult FindClass(int id)
        {
            //Getting data from DB
            WoWClass Class = db.Classes.Find(id);
            //returns 404 if not found
            if (Class == null)
            {
                return NotFound();
            }

            //DTO
            WoWClass ClassDto = new WoWClass
            {
                ClassID = Class.ClassID,
                ClassName = Class.ClassName,
                ClassPic = Class.ClassPic,
            };

            return Ok(ClassDto);


        }

        /// <summary>
        /// Update a class in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateClass(int id, [FromBody] WoWClass Class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Class.ClassID)
            {
                return BadRequest();
            }

            db.Entry(Class).State = EntityState.Modified;
            //Picture update handled by UpdateClassPic
            db.Entry(Class).Property(p => p.ClassPic).IsModified = false;
            db.Entry(Class).Property(p => p.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        ///<summary>
        ///Take player picture data, uploads it to server, then updates Class pic
        /// </summary>
        /// <param name="id">Class ID</param>
        /// <returns>Status code 200 if success</returns>
        /// <example>
        /// 
        /// </example>
        /// 

        [HttpPost]
        public IHttpActionResult UpdateClassPic(int id)
        {
            bool haspic = false;
            string picextension;

            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //check if file has been uploaded
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var ClassPic = HttpContext.Current.Request.Files[0];
                    //checking if file is empty
                    if (ClassPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(ClassPic.FileName).Substring(1);
                        //Finding the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //getting direct file path to ~/Content/Class/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Classes/"), fn);

                                //saving file
                                ClassPic.SaveAs(path);

                                //if successful, set fields
                                haspic = true;
                                picextension = extension;

                                //Update fields in the database!
                                WoWClass SelectedClass = db.Classes.Find(id);
                                SelectedClass.ClassPic = haspic;
                                SelectedClass.PicExtension = extension;
                                db.Entry(SelectedClass).State = EntityState.Modified;

                                db.SaveChanges();


                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Class picture was not saved");
                                Debug.WriteLine("Exception: " + ex);
                            }
                        }
                    }
                }
            }
            return Ok();

        }




        /// <summary>
        /// Delete a class from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteClass(int id)
        {
            WoWClass @class = db.Classes.Find(id);
            if (@class == null)
            {
                return NotFound();
            }

            db.Classes.Remove(@class);
            db.SaveChanges();

            return Ok(@class);
        }

        /// <summary>
        /// Add a class to the database
        /// </summary>
        /// <param name="disposing"></param>
        [ResponseType(typeof(WoWClass))]
        public IHttpActionResult AddClass([FromBody] WoWClass Class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            db.Classes.Add(Class);
            db.SaveChanges();
            

            return Ok(Class.ClassID);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassExists(int id)
        {
            return db.Classes.Count(e => e.ClassID == id) > 0;
        }

    }  
}