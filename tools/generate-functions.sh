# Generate functions list from header file
{
  printf '# LIBMDBX_API functions\n\n'

  awk '
    BEGIN { skip_pp = 0 }

    { sub(/\r$/, "") }

    skip_pp {
      if ($0 !~ /\\[[:space:]]*$/) skip_pp = 0
      next
    }

    /^[[:space:]]*#/ {
      if ($0 ~ /\\[[:space:]]*$/) skip_pp = 1
      next
    }

    /(^|[^A-Za-z0-9_])LIBMDBX_API([^A-Za-z0-9_]|$)/ {
      decl = $0

      while (decl !~ /;/) {
        if ((getline line) <= 0) break
        sub(/\r$/, "", line)
        decl = decl " " line
      }

      print decl
    }
  ' ./headers/mdbx.h \
    | grep -oE '\bmdbx_[A-Za-z0-9_]+[[:space:]]*\(' \
    | sed 's/[[:space:]]*($//' \
    | sort -u \
    | sed 's/^/- `/' \
    | sed 's/$/`/'
} > ../LIBMDBX-FUNCTIONS.md