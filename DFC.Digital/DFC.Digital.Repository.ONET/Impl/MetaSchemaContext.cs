﻿namespace DFC.Digital.Repository.ONET.Impl
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using DataModel;
    using Interface;

    public class MetaSchemaContext : DbContext, IDbContext
    {
        public MetaSchemaContext() : base("name=JobProfileFrameWorksEntities1")
        {
            Database.CommandTimeout = 60;
        }

        public new IDbSetWrapper<TEntity> Set<TEntity>() where TEntity : class
        {
            var set = base.Set<TEntity>();
            return new DbSetWrapper<TEntity>(set);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<content_model_reference> content_model_reference { get; set; }
        public virtual DbSet<DFC_GDSCombinations> DFC_GDSCombinations { get; set; }
        public virtual DbSet<DFC_GDSContext> DFC_GDSContext { get; set; }
        public virtual DbSet<DFC_GDSTranlations> DFC_GDSTranlations { get; set; }
        public virtual DbSet<DFC_GlobalAttributeSuppression> DFC_GlobalAttributeSuppression { get; set; }
        public virtual DbSet<dwa_reference> dwa_reference { get; set; }
        public virtual DbSet<ete_categories> ete_categories { get; set; }
        public virtual DbSet<green_dwa_reference> green_dwa_reference { get; set; }
        public virtual DbSet<iwa_reference> iwa_reference { get; set; }
        public virtual DbSet<job_zone_reference> job_zone_reference { get; set; }
        public virtual DbSet<occupation_data> occupation_data { get; set; }
        public virtual DbSet<scales_reference> scales_reference { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<task_categories> task_categories { get; set; }
        public virtual DbSet<task_statements> task_statements { get; set; }
        public virtual DbSet<unspsc_reference> unspsc_reference { get; set; }
        public virtual DbSet<work_context_categories> work_context_categories { get; set; }
        public virtual DbSet<ability> abilities { get; set; }
        public virtual DbSet<alternate_titles> alternate_titles { get; set; }
        public virtual DbSet<career_changers_matrix> career_changers_matrix { get; set; }
        public virtual DbSet<career_starters_matrix> career_starters_matrix { get; set; }
        public virtual DbSet<DFC_SocMappings> DFC_SocMappings { get; set; }
        public virtual DbSet<education_training_experience> education_training_experience { get; set; }
        public virtual DbSet<emerging_tasks> emerging_tasks { get; set; }
        public virtual DbSet<green_occupations> green_occupations { get; set; }
        public virtual DbSet<green_task_statements> green_task_statements { get; set; }
        public virtual DbSet<interest> interests { get; set; }
        public virtual DbSet<job_zones> job_zones { get; set; }
        public virtual DbSet<knowledge> knowledges { get; set; }
        public virtual DbSet<level_scale_anchors> level_scale_anchors { get; set; }
        public virtual DbSet<occupation_level_metadata> occupation_level_metadata { get; set; }
        public virtual DbSet<sample_of_reported_titles> sample_of_reported_titles { get; set; }
        public virtual DbSet<skill> skills { get; set; }
        public virtual DbSet<survey_booklet_locations> survey_booklet_locations { get; set; }
        public virtual DbSet<task_ratings> task_ratings { get; set; }
        public virtual DbSet<tasks_to_dwas> tasks_to_dwas { get; set; }
        public virtual DbSet<tasks_to_green_dwas> tasks_to_green_dwas { get; set; }
        public virtual DbSet<tools_and_technology> tools_and_technology { get; set; }
        public virtual DbSet<work_activities> work_activities { get; set; }
        public virtual DbSet<work_context> work_context { get; set; }
        public virtual DbSet<work_styles> work_styles { get; set; }
        public virtual DbSet<work_values> work_values { get; set; }

        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));

            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }

        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));

            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }

        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }

        public virtual ObjectResult<sp_GetAttributesByOnNetCode_Result> sp_GetAttributesByOnNetCode(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAttributesByOnNetCode_Result>("sp_GetAttributesByOnNetCode", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetAttributesByOnNetCodeForEachSectionGrouped_Result> sp_GetAttributesByOnNetCodeForEachSectionGrouped(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAttributesByOnNetCodeForEachSectionGrouped_Result>("sp_GetAttributesByOnNetCodeForEachSectionGrouped", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetAttributesByOnNetCodeForEachSectionGroupedV2_Result> sp_GetAttributesByOnNetCodeForEachSectionGroupedV2(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAttributesByOnNetCodeForEachSectionGroupedV2_Result>("sp_GetAttributesByOnNetCodeForEachSectionGroupedV2", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetAttributesByOnNetCodeGrouped_Result> sp_GetAttributesByOnNetCodeGrouped(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAttributesByOnNetCodeGrouped_Result>("sp_GetAttributesByOnNetCodeGrouped", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetAttributesByOnNetCodeGroupedV2_Result> sp_GetAttributesByOnNetCodeGroupedV2(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAttributesByOnNetCodeGroupedV2_Result>("sp_GetAttributesByOnNetCodeGroupedV2", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetInterestsByOnetCode_Result> sp_GetInterestsByOnetCode(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetInterestsByOnetCode_Result>("sp_GetInterestsByOnetCode", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetTechnologyDetailsByOnetCode_Result> sp_GetTechnologyDetailsByOnetCode(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetTechnologyDetailsByOnetCode_Result>("sp_GetTechnologyDetailsByOnetCode", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetToolsTecByOnetCode_Result> sp_GetToolsTecByOnetCode(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetToolsTecByOnetCode_Result>("sp_GetToolsTecByOnetCode", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetToolsTecByOnetCodeV2_Result> sp_GetToolsTecByOnetCodeV2(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetToolsTecByOnetCodeV2_Result>("sp_GetToolsTecByOnetCodeV2", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetWorkStylesByOnetCode_Result> sp_GetWorkStylesByOnetCode(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetWorkStylesByOnetCode_Result>("sp_GetWorkStylesByOnetCode", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_GetWorkValuesByOnetCode_Result> sp_GetWorkValuesByOnetCode(string oNetCode)
        {
            var oNetCodeParameter = oNetCode != null ?
                new ObjectParameter("ONetCode", oNetCode) :
                new ObjectParameter("ONetCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetWorkValuesByOnetCode_Result>("sp_GetWorkValuesByOnetCode", oNetCodeParameter);
        }

        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }

        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }

        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }

        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}
