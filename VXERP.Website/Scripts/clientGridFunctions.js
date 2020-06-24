function OnRowClick(s, e) {
    s.SelectRowOnPage(e.visibleIndex, !s.IsRowSelectedOnPage(e.visibleIndex));
}