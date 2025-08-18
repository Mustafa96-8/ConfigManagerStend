using ConfigManagerStend.Domain.Entities;

namespace ConfigManagerStend.Domain.Predefineds
{
    /// <summary>
    /// Предопределенные данные. Репо
    /// </summary>
    internal class PdBuildDefinition
    {
        #region DELO REPOS
        internal BuildDefinition d_box = new()
        {
            Id = 1,
            ProjectId = 1,
            NameRepo = "Delo2020"
        };
        internal BuildDefinition d_cb = new()
        {
            Id = 2,
            ProjectId = 1,
            NameRepo = "Delo2020-CB"
        };
        internal BuildDefinition d_eek = new()
        {
            Id = 3,
            ProjectId = 1,
            NameRepo = "Delo2020-EEK"
        };
        internal BuildDefinition d_gd = new()
        {
            Id = 4,
            ProjectId = 1,
            NameRepo = "Delo2020-GD"
        };
        internal BuildDefinition d_kalin = new()
        {
            Id = 5,
            ProjectId = 1,
            NameRepo = "Delo2020-Kaliningrad"
        };
        internal BuildDefinition d_minkult = new()
        {
            Id = 6,
            ProjectId = 1,
            NameRepo = "Delo2020-MinKult"
        };
        internal BuildDefinition d_mintrud = new()
        {
            Id = 7,
            ProjectId = 1,
            NameRepo = "Delo2020-MinTrud"
        };
        #endregion

        #region TITUL REPOS
        internal BuildDefinition t_box = new()
        {
            Id = 8,
            ProjectId = 2,
            NameRepo = "TITUL"
        };
        #endregion

        #region NADZOR REPOS
        internal BuildDefinition n_box = new()
        {
            Id = 9,
            ProjectId = 3,
            NameRepo = "Nadzor2025"
        };
        #endregion

        internal List<BuildDefinition> Repos;

        internal PdBuildDefinition()
        {
            Repos = new()
            {
                d_box, d_cb, d_eek, d_gd, d_kalin, d_minkult, d_mintrud, t_box, n_box
            };
        }
    }
}
