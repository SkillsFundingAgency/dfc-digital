﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Interface;
namespace DFC.Digital.Repository.ONET.Impl
{
    public class OnetRepositoryDbContext : DbContext, IDbContext
    {
        public OnetRepositoryDbContext() : base("name=JobProfileFrameWorksEntities")
        {
            Database.CommandTimeout = 60;
        }
        public virtual DbSet<ability> abilities { get; set; }
        public virtual DbSet<alternate_titles> alternate_titles { get; set; }
        public virtual DbSet<career_changers_matrix> career_changers_matrix { get; set; }
        public virtual DbSet<career_starters_matrix> career_starters_matrix { get; set; }
        public virtual DbSet<content_model_reference> content_model_reference { get; set; }
        public virtual DbSet<DFC_GDSCombinations> DFC_GDSCombinations { get; set; }
        public virtual DbSet<DFC_GDSContext> DFC_GDSContext { get; set; }
        public virtual DbSet<DFC_GDSTranlations> DFC_GDSTranlations { get; set; }
        public virtual DbSet<DFC_GlobalAttributeSuppression> DFC_GlobalAttributeSuppression { get; set; }
        public virtual DbSet<DFC_SocMappings> DFC_SocMappings { get; set; }
        public virtual DbSet<dwa_reference> dwa_reference { get; set; }
        public virtual DbSet<education_training_experience> education_training_experience { get; set; }
        public virtual DbSet<emerging_tasks> emerging_tasks { get; set; }
        public virtual DbSet<ete_categories> ete_categories { get; set; }
        public virtual DbSet<green_dwa_reference> green_dwa_reference { get; set; }
        public virtual DbSet<green_occupations> green_occupations { get; set; }
        public virtual DbSet<green_task_statements> green_task_statements { get; set; }
        public virtual DbSet<interest> interests { get; set; }
        public virtual DbSet<iwa_reference> iwa_reference { get; set; }
        public virtual DbSet<job_zone_reference> job_zone_reference { get; set; }
        public virtual DbSet<job_zones> job_zones { get; set; }
        public virtual DbSet<knowledge> knowledges { get; set; }
        public virtual DbSet<level_scale_anchors> level_scale_anchors { get; set; }
        public virtual DbSet<occupation_data> occupation_data { get; set; }
        public virtual DbSet<occupation_level_metadata> occupation_level_metadata { get; set; }
        public virtual DbSet<sample_of_reported_titles> sample_of_reported_titles { get; set; }
        public virtual DbSet<scales_reference> scales_reference { get; set; }
        public virtual DbSet<skill> skills { get; set; }
        public virtual DbSet<survey_booklet_locations> survey_booklet_locations { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<task_categories> task_categories { get; set; }
        public virtual DbSet<task_ratings> task_ratings { get; set; }
        public virtual DbSet<task_statements> task_statements { get; set; }
        public virtual DbSet<tasks_to_dwas> tasks_to_dwas { get; set; }
        public virtual DbSet<tasks_to_green_dwas> tasks_to_green_dwas { get; set; }
        public virtual DbSet<tools_and_technology> tools_and_technology { get; set; }
        public virtual DbSet<unspsc_reference> unspsc_reference { get; set; }
        public virtual DbSet<work_activities> work_activities { get; set; }
        public virtual DbSet<work_context> work_context { get; set; }
        public virtual DbSet<work_context_categories> work_context_categories { get; set; }
        public virtual DbSet<work_styles> work_styles { get; set; }
        public virtual DbSet<work_values> work_values { get; set; }

        public new IDbSetWrapper<TEntity> Set<TEntity>() where TEntity : class
        {
            var set = base.Set<TEntity>();
            return new DbSetWrapper<TEntity>(set);
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    }
}
