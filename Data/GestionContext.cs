using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace gestion.site.Data
{
    public partial class GestionContext : DbContext
    {
        public GestionContext()
        {
        }

        public GestionContext(DbContextOptions<GestionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<area> area { get; set; }
        public virtual DbSet<configuracion> configuracion { get; set; }
        public virtual DbSet<criticidad> criticidad { get; set; }
        public virtual DbSet<especialista> especialista { get; set; }
        public virtual DbSet<estado> estado { get; set; }
        public virtual DbSet<estado_final> estado_final { get; set; }
        public virtual DbSet<estado_inicial> estado_inicial { get; set; }
        public virtual DbSet<relacion_estado> relacion_estado { get; set; }
        public virtual DbSet<rol> rol { get; set; }
        public virtual DbSet<solicitante> solicitante { get; set; }
        public virtual DbSet<tarea> tarea { get; set; }
        public virtual DbSet<tarea_especialista> tarea_especialista { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<area>(entity =>
            {
                entity.HasKey(e => e.area_id);

                entity.Property(e => e.area_desc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<configuracion>(entity =>
            {
                entity.HasKey(e => e.configuracion_cod);

                entity.Property(e => e.configuracion_cod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.configuracion_desc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.valor_numerico).HasColumnType("numeric(18, 10)");

                entity.Property(e => e.valor_texto).IsUnicode(false);
            });

            modelBuilder.Entity<criticidad>(entity =>
            {
                entity.HasKey(e => e.criticidad_id);

                entity.Property(e => e.criticidad_desc)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<especialista>(entity =>
            {
                entity.HasKey(e => e.especialista_id);

                entity.HasIndex(e => e.usuario, "IX_especialista")
                    .IsUnique();

                entity.Property(e => e.contrasena)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode();

                entity.Property(e => e.correo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.especialista_desc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.rol_cod)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.usuario)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.rol_codNavigation)
                    .WithMany(p => p.especialista)
                    .HasForeignKey(d => d.rol_cod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_especialista_rol");
            });

            modelBuilder.Entity<estado>(entity =>
            {
                entity.HasKey(e => e.estado_id);

                entity.Property(e => e.estado_desc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<estado_final>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("estado_final");

                entity.Property(e => e.estado_desc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<estado_inicial>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("estado_inicial");

                entity.Property(e => e.estado_desc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<relacion_estado>(entity =>
            {
                entity.HasKey(e => new { e.estado_inicial_id, e.estado_final_id });

                entity.HasOne(d => d.estado_final)
                    .WithMany(p => p.relacion_estadoestado_final)
                    .HasForeignKey(d => d.estado_final_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_relacion_estado_estado1");

                entity.HasOne(d => d.estado_inicial)
                    .WithMany(p => p.relacion_estadoestado_inicial)
                    .HasForeignKey(d => d.estado_inicial_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_relacion_estado_estado");
            });

            modelBuilder.Entity<rol>(entity =>
            {
                entity.HasKey(e => e.rol_cod);

                entity.Property(e => e.rol_cod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.rol_desc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<solicitante>(entity =>
            {
                entity.HasKey(e => e.solicitante_id);

                entity.Property(e => e.correo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.solicitante_desc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tarea>(entity =>
            {
                entity.HasKey(e => e.tarea_id);

                entity.Property(e => e.fecha_asignacion).HasColumnType("datetime");

                entity.Property(e => e.fecha_compromiso).HasColumnType("datetime");

                entity.Property(e => e.fecha_solicitud).HasColumnType("datetime");

                entity.Property(e => e.fecha_solucion).HasColumnType("datetime");

                entity.Property(e => e.tarea_desc)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.area)
                    .WithMany(p => p.tarea)
                    .HasForeignKey(d => d.area_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tarea_area");

                entity.HasOne(d => d.creador)
                    .WithMany(p => p.tarea)
                    .HasForeignKey(d => d.creador_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tarea_especialista");

                entity.HasOne(d => d.criticidad)
                    .WithMany(p => p.tarea)
                    .HasForeignKey(d => d.criticidad_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tarea_criticidad");

                entity.HasOne(d => d.estado)
                    .WithMany(p => p.tarea)
                    .HasForeignKey(d => d.estado_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tarea_estado");

                entity.HasOne(d => d.solicitante)
                    .WithMany(p => p.tarea)
                    .HasForeignKey(d => d.solicitante_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tarea_solicitante");
            });

            modelBuilder.Entity<tarea_especialista>(entity =>
            {
                entity.HasKey(e => new { e.tarea_id, e.especialista_id });

                entity.HasOne(d => d.especialista)
                    .WithMany(p => p.tarea_especialista)
                    .HasForeignKey(d => d.especialista_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tarea_especialista_especialista");

                entity.HasOne(d => d.tarea)
                    .WithMany(p => p.tarea_especialista)
                    .HasForeignKey(d => d.tarea_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tarea_especialista_tarea");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
