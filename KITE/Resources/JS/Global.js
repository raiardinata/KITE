'use strict';  // http://ejohn.org/blog/ecmascript-5-strict-mode-json-and-more/

$(function() {
  // Make the user aware of keyboard shortcuts
  $("button[accesskey], a[accesskey]").each(function (i, tag) {
    if (tag.accessKey)
      tag.title += ' [' +
        (tag.accessKeyLabel || tag.accessKey).replace('Shift', '⇧') + ']';
  });

  /* Protect all anchors and buttons against double-clicking.
   * The timeout in milliseconds is given by the *data-reclick* attribute.
   * If the value is not present or not an integer,
   * the default of 1000 is used. */
  $('a, button, input[type=submit]').live('click', function(e) {
    var o = $(this);
    var reclick = parseInt(o.data('reclick')) || 1000;
    if (o.hasClass('disabled'))  e.preventDefault();  // don't follow anchor
    if (reclick) {
      o.attr('disabled', true);
      o.addClass('disabled');
      setTimeout(function () {
        o.removeAttr('disabled');
        o.removeClass('disabled');
      }, reclick);
    }
  });
});

