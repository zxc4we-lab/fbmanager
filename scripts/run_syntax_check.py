#!/usr/bin/env python3
"""
Perform a basic syntax check on C# and Razor files.  This script does not
compile the project but instead ensures that all brackets, braces and
parentheses are balanced.  While not a substitute for a real compiler,
this check can catch many common typos that would otherwise cause the
application to fail at runtime.
"""
import os
import sys

PROJECT_ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), '..'))

def check_file(path: str) -> bool:
    """Return True if file has balanced brackets, False otherwise."""
    pairs = {')': '(', '}': '{', ']': '['}
    opening = pairs.values()
    stack = []
    with open(path, 'r', encoding='utf-8') as f:
        for line_no, line in enumerate(f, 1):
            for ch in line:
                if ch in opening:
                    stack.append((ch, line_no))
                elif ch in pairs:
                    if not stack:
                        print(f"Unmatched closing '{ch}' at {path}:{line_no}")
                        return False
                    last_open, last_line = stack.pop()
                    if pairs[ch] != last_open:
                        print(f"Mismatched '{last_open}' opened at line {last_line} closed by '{ch}' at line {line_no} in {path}")
                        return False
    if stack:
        last_open, last_line = stack[-1]
        print(f"Unclosed '{last_open}' opened at line {last_line} in {path}")
        return False
    return True

def main() -> int:
    # Collect all .cs and .cshtml files in the project
    failed = False
    for root, dirs, files in os.walk(PROJECT_ROOT):
        for file in files:
            if file.endswith('.cs') or file.endswith('.cshtml'):
                path = os.path.join(root, file)
                if not check_file(path):
                    failed = True
    if failed:
        print("Syntax check failed")
        return 1
    print("All files passed basic syntax check")
    return 0

if __name__ == '__main__':
    sys.exit(main())